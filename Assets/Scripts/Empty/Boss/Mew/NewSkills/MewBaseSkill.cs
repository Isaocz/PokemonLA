using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 梦幻 Boss 技能基础类。
///
/// 生命周期：Initialize -> Startup -> CoreLogic -> Endup -> Finished。
/// 旧版 Mew.cs 仍可使用 skill.SetEmpty(this)，新强技能应优先使用 Initialize(context)。
/// </summary>
public abstract class MewBaseSkill : MonoBehaviour, IMewSkill
{
    #region Inspector

    [Header("技能基础信息")]
    [SerializeField] protected string skillName = "Mew Skill";

    [Header("技能时间（只按以下顺序执行）")]
    [Tooltip("通用前摇：技能对象初始化后、CoreLogic 开始前等待。若技能内部已有专用预警（例如星之冲刺的音符预警），这里设为 0。")]
    [SerializeField, Min(0f)] protected float skillStartup;

    [Tooltip("通用后摇：CoreLogic 完全结束后、Mew 允许开始下一次传送前等待。")]
    [SerializeField, Min(0f)] protected float skillEndup;

    [Tooltip("仅控制技能承载 GameObject 在逻辑结束后多久销毁；不会阻止 Mew 释放下一技能。第一阶段技能通常设为 0。")]
    [SerializeField, Min(0f)] protected float existTime;

    // 六个基础技能的轮次都由各自 CoreLogic 内部管理。
    // 保留字段只为兼容旧 Prefab，不再在 Inspector 中作为节奏参数使用。
    [SerializeField, HideInInspector, Min(0f)] protected float repeatTime;
    [SerializeField, HideInInspector, Min(1)] protected int repeat = 1;

    [Header("执行与清理")]
    [Tooltip("兼容旧版 Mew.cs：调用 SetEmpty(this) 后自动执行技能。")]
    [SerializeField] private bool executeImmediatelyWhenSetEmpty = true;

    [Tooltip("技能正常结束后自动销毁技能承载对象。")]
    [SerializeField] private bool destroyOnFinish = true;

    [Tooltip("技能正常结束时，自动清理通过 RegisterTemporaryObject 注册的临时对象。")]
    [SerializeField] private bool cleanupTemporaryObjectsOnFinish;

    [Tooltip("Startup、Repeat、Endup 是否使用不受 Time.timeScale 影响的时间。")]
    [SerializeField] private bool useUnscaledLifecycleTime;

    #endregion

    #region 兼容字段与运行上下文

    // 保留 protected empty，避免现有 StarScatter、StampStar 等技能全部报错。
    protected Empty empty;
    protected bool isCasting;
    protected float lastCastTime = -1f;

    protected Transform playerTransform;
    protected Transform emitPoint;
    protected Transform arenaTransform;
    protected Vector2 arenaCenter;
    protected float arenaRadius;
    protected MewSkillPhase skillPhase = MewSkillPhase.Phase1;

    private readonly List<GameObject> temporaryObjects = new List<GameObject>();

    private Coroutine executionCoroutine;
    private MewSkillState state = MewSkillState.Uninitialized;
    private bool isInitialized;
    private bool cancelRequested;
    private bool finishEventRaised;
    private int currentRepeatIndex = -1;

    #endregion

    #region IMewSkill 属性

    public string SkillName => skillName;
    public float SkillStartup => skillStartup;
    public float SkillEndup => skillEndup;
    public float ExistTime => existTime;
    public int Repeat => repeat;
    public float RepeatTime => repeatTime;

    public Empty Caster => empty;
    public Transform Player => GetPlayerTransform();
    public Transform EmitPoint => emitPoint != null ? emitPoint : transform;
    public Transform ArenaTransform => arenaTransform;

    /// <summary>
    /// 弹幕默认应从技能对象/显式发射点生成，而不是偷偷改用 Boss 当前坐标。
    /// </summary>
    public Vector3 SkillOrigin => EmitPoint != null ? EmitPoint.position : transform.position;

    public Vector2 ArenaCenter => arenaCenter;
    public float ArenaRadius => arenaRadius;
    public MewSkillPhase Phase => skillPhase;
    public MewSkillState State => state;
    public int CurrentRepeatIndex => currentRepeatIndex;

    public bool IsInitialized => isInitialized;
    public bool IsCasting => isCasting;
    public bool IsFinished =>
        state == MewSkillState.Completed ||
        state == MewSkillState.Cancelled ||
        state == MewSkillState.Failed ||
        state == MewSkillState.Destroyed;

    public event Action<IMewSkill, MewSkillFinishReason> Finished;

    #endregion

    #region 初始化

    /// <summary>
    /// 推荐初始化方式，尤其适合需要固定限制圈中心的二阶段强技能。
    /// </summary>
    public virtual void Initialize(
        MewSkillContext context,
        bool executeImmediately = true)
    {
        if (IsCasting)
        {
            Debug.LogWarning(
                $"{name}：技能正在执行，不能重复 Initialize。",
                this);
            return;
        }

        if (context.Caster == null)
        {
            FailInitialization("技能上下文没有设置 Caster/Empty。");
            return;
        }

        empty = context.Caster;
        skillPhase = ClampPhase(context.Phase);
        playerTransform = context.Player != null
            ? context.Player
            : FindPlayerTransform();
        emitPoint = context.EmitPoint != null
            ? context.EmitPoint
            : transform;
        arenaTransform = context.ArenaTransform;
        arenaCenter = context.ArenaCenter;
        arenaRadius = Mathf.Max(0f, context.ArenaRadius);

        cancelRequested = false;
        finishEventRaised = false;
        currentRepeatIndex = -1;
        isCasting = false;
        isInitialized = true;
        state = MewSkillState.Ready;

        OnContextInitialized(context);

        if (executeImmediately)
        {
            Execute();
        }
    }

    /// <summary>
    /// 兼容现有 Mew.cs 的 SetEmpty(this) 调用。
    /// 会从 Mew.currentPhase 推断阶段，并把技能对象自身作为默认发射点。
    /// </summary>
    public virtual void SetEmpty(Empty caster)
    {
        if (caster == null)
        {
            FailInitialization("SetEmpty 收到了空的施法者。");
            return;
        }

        Transform foundPlayer = FindPlayerTransform();
        MewSkillPhase inferredPhase = InferPhase(caster);

        Vector2 inferredArenaCenter = caster.transform.parent != null
            ? (Vector2)caster.transform.parent.position
            : (Vector2)caster.transform.position;

        MewSkillContext context = new MewSkillContext(
            caster,
            inferredPhase,
            foundPlayer,
            transform,
            inferredArenaCenter);

        Initialize(context, executeImmediatelyWhenSetEmpty);
    }

    protected virtual void OnContextInitialized(MewSkillContext context)
    {
    }

    protected virtual MewSkillPhase InferPhase(Empty caster)
    {
        Mew mew = caster as Mew;
        if (mew == null)
        {
            return MewSkillPhase.Phase1;
        }

        return (MewSkillPhase)Mathf.Clamp(mew.currentPhase, 1, 3);
    }

    private static MewSkillPhase ClampPhase(MewSkillPhase phase)
    {
        int value = Mathf.Clamp((int)phase, 1, 3);
        return (MewSkillPhase)value;
    }

    private void FailInitialization(string message)
    {
        Debug.LogError($"{name}：{message}", this);
        isInitialized = false;
        isCasting = false;
        state = MewSkillState.Failed;
        RaiseFinished(MewSkillFinishReason.Failed);

        if (destroyOnFinish)
        {
            Destroy(gameObject, Mathf.Max(0f, existTime));
        }
    }

    #endregion

    #region 执行生命周期

    public void Execute()
    {
        if (!isInitialized)
        {
            FailInitialization(
                "技能尚未初始化。请先调用 SetEmpty 或 Initialize。");
            return;
        }

        if (IsCasting)
        {
            Debug.LogWarning($"{name}：技能已在执行，忽略重复 Execute。", this);
            return;
        }

        if (IsFinished)
        {
            Debug.LogWarning(
                $"{name}：技能已经结束，如需再次释放请重新实例化技能对象。",
                this);
            return;
        }

        executionCoroutine = StartCoroutine(ExecuteWithTime());
    }

    public IEnumerator ExecuteWithTime()
    {
        if (!isInitialized || IsCasting || IsFinished)
        {
            yield break;
        }

        isCasting = true;
        cancelRequested = false;
        state = MewSkillState.Startup;
        OnSkillStarted();

        yield return Startup();
        if (cancelRequested)
        {
            yield break;
        }

        int executionCount = GetExecutionCount();

        for (int i = 0; i < executionCount; i++)
        {
            currentRepeatIndex = i;
            state = MewSkillState.Executing;
            OnRepeatStarted(i, executionCount);

            yield return CoreLogic();
            if (cancelRequested)
            {
                yield break;
            }

            if (i < executionCount - 1)
            {
                state = MewSkillState.RepeatInterval;
                yield return SkillRepeat();

                if (cancelRequested)
                {
                    yield break;
                }
            }
        }

        state = MewSkillState.Endup;
        yield return Endup();

        if (cancelRequested)
        {
            yield break;
        }

        CompleteNormally();
    }

    public IEnumerator WaitForCompletion()
    {
        while (!IsFinished)
        {
            yield return null;
        }
    }

    protected virtual int GetExecutionCount()
    {
        return Mathf.Max(1, repeat);
    }

    protected virtual void OnSkillStarted()
    {
    }

    protected virtual void OnRepeatStarted(int repeatIndex, int totalRepeatCount)
    {
    }

    protected virtual void OnSkillFinished(MewSkillFinishReason reason)
    {
    }

    private void CompleteNormally()
    {
        executionCoroutine = null;
        isCasting = false;
        lastCastTime = Time.time;
        state = MewSkillState.Completed;

        if (cleanupTemporaryObjectsOnFinish)
        {
            CleanupTemporaryObjects();
        }

        RaiseFinished(MewSkillFinishReason.Completed);

        if (destroyOnFinish)
        {
            Destroy(gameObject, Mathf.Max(0f, existTime));
        }
    }

    public virtual void Cancel(bool destroyObject = true)
    {
        if (IsFinished)
        {
            if (destroyObject && this != null)
            {
                Destroy(gameObject);
            }
            return;
        }

        cancelRequested = true;
        executionCoroutine = null;

        // 停止技能对象上所有由派生类启动的协程，
        // 例如旋转星光的持续散射、强技能的分身转圈等。
        StopAllCoroutines();

        isCasting = false;
        state = MewSkillState.Cancelled;
        CleanupTemporaryObjects();
        RaiseFinished(MewSkillFinishReason.Cancelled);

        if (destroyObject)
        {
            Destroy(gameObject);
        }
    }

    private void RaiseFinished(MewSkillFinishReason reason)
    {
        if (finishEventRaised)
        {
            return;
        }

        finishEventRaised = true;

        try
        {
            OnSkillFinished(reason);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception, this);
        }

        try
        {
            Finished?.Invoke(this, reason);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception, this);
        }
    }

    #endregion

    #region 可覆写生命周期段

    /// <summary>
    /// 核心逻辑。每次 Repeat 都会执行一次。
    /// 对星环折返这类“内部包含多圈”的技能，Repeat 应保持 1，
    /// 圈数由技能脚本自身的 waveCount 控制。
    /// </summary>
    public abstract IEnumerator CoreLogic();

    public virtual IEnumerator Startup()
    {
        return WaitLifecycle(skillStartup);
    }

    public virtual IEnumerator SkillRepeat()
    {
        return WaitLifecycle(repeatTime);
    }

    public virtual IEnumerator Endup()
    {
        return WaitLifecycle(skillEndup);
    }

    protected IEnumerator WaitLifecycle(float duration)
    {
        if (duration <= 0f)
        {
            yield break;
        }

        if (!useUnscaledLifecycleTime)
        {
            yield return new WaitForSeconds(duration);
            yield break;
        }

        float endTime = Time.unscaledTime + duration;
        while (Time.unscaledTime < endTime)
        {
            if (cancelRequested)
            {
                yield break;
            }

            yield return null;
        }
    }

    #endregion

    #region 玩家、施法者与阶段辅助

    public Transform GetPlayerTransform()
    {
        if (playerTransform != null)
        {
            return playerTransform;
        }

        playerTransform = FindPlayerTransform();
        return playerTransform;
    }

    private static Transform FindPlayerTransform()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        return playerObject != null ? playerObject.transform : null;
    }

    public Vector2 GetPlayerDirection()
    {
        return GetPlayerDirection(SkillOrigin);
    }

    public Vector2 GetPlayerDirection(Vector3 origin)
    {
        Transform target = GetPlayerTransform();
        if (target == null)
        {
            return Vector2.zero;
        }

        Vector2 direction = (Vector2)(target.position - origin);
        return direction.sqrMagnitude > 0.0001f
            ? direction.normalized
            : Vector2.zero;
    }

    protected Vector2 GetDirection(Vector3 origin, Vector3 target)
    {
        Vector2 direction = (Vector2)(target - origin);
        return direction.sqrMagnitude > 0.0001f
            ? direction.normalized
            : Vector2.zero;
    }

    /// <summary>
    /// 根据阶段选择参数，适合实现文档中的二阶段强化。
    /// 例如：int count = PhaseValue(4, 6, 6);
    /// </summary>
    protected T PhaseValue<T>(T phase1Value, T phase2Value, T phase3Value)
    {
        switch (skillPhase)
        {
            case MewSkillPhase.Phase2:
                return phase2Value;
            case MewSkillPhase.Phase3:
                return phase3Value;
            default:
                return phase1Value;
        }
    }

    protected bool IsPhase(MewSkillPhase phase)
    {
        return skillPhase == phase;
    }

    protected bool IsPhase2OrLater => (int)skillPhase >= (int)MewSkillPhase.Phase2;

    /// <summary>
    /// 取得固定限制圈上的点，供镜像星光、天象时钟、拟态群星等强技能使用。
    /// </summary>
    protected Vector3 GetArenaPoint(float angleDegrees, float radius = -1f)
    {
        float usedRadius = radius >= 0f ? radius : arenaRadius;
        Vector2 direction = Quaternion.Euler(0f, 0f, angleDegrees) * Vector2.right;
        return arenaCenter + direction * usedRadius;
    }

    #endregion

    #region 临时对象清理

    /// <summary>
    /// 注册技能结束/取消时需要一起清理的临时对象。
    /// 适合分身、预警图形、时钟指针、技能专属限制圈；
    /// 不要把需要在技能结束后继续飞行的普通弹幕注册进来。
    /// </summary>
    protected void RegisterTemporaryObject(GameObject target)
    {
        if (target != null && !temporaryObjects.Contains(target))
        {
            temporaryObjects.Add(target);
        }
    }

    protected GameObject SpawnTemporaryObject(
        GameObject prefab,
        Vector3 position,
        Quaternion rotation,
        Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError($"{name}：尝试生成空的临时对象 Prefab。", this);
            return null;
        }

        GameObject instance = Instantiate(prefab, position, rotation, parent);
        RegisterTemporaryObject(instance);
        return instance;
    }

    protected void UnregisterTemporaryObject(GameObject target)
    {
        temporaryObjects.Remove(target);
    }

    protected void CleanupTemporaryObjects()
    {
        for (int i = temporaryObjects.Count - 1; i >= 0; i--)
        {
            GameObject target = temporaryObjects[i];
            if (target != null)
            {
                Destroy(target);
            }
        }

        temporaryObjects.Clear();
    }

    #endregion

    #region 向后兼容的销毁方法

    public void SelfDestroy()
    {
        if (!IsFinished)
        {
            StopAllCoroutines();
            isCasting = false;
            state = MewSkillState.Completed;
            RaiseFinished(MewSkillFinishReason.Completed);
        }

        Destroy(gameObject, Mathf.Max(0f, existTime));
    }

    // 保留原项目中的拼写，避免旧技能脚本调用时报错。
    public void SelfDestory()
    {
        SelfDestroy();
    }

    protected virtual void OnDestroy()
    {
        if (finishEventRaised || IsFinished)
        {
            return;
        }

        isCasting = false;
        state = MewSkillState.Destroyed;
        RaiseFinished(MewSkillFinishReason.Destroyed);
    }

    #endregion
}

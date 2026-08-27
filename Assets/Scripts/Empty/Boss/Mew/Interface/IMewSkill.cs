using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 梦幻 Boss 的技能阶段。
/// 不直接使用 int，可避免技能脚本中出现难以理解的 1、2、3 判断。
/// </summary>
public enum MewSkillPhase
{
    Phase1 = 1,
    Phase2 = 2,
    Phase3 = 3
}

/// <summary>
/// 技能当前所处的生命周期状态。
/// </summary>
public enum MewSkillState
{
    Uninitialized,
    Ready,
    Startup,
    Executing,
    RepeatInterval,
    Endup,
    Completed,
    Cancelled,
    Failed,
    Destroyed
}

/// <summary>
/// 技能结束原因，供 Mew 的技能调度器判断是否可以继续下一技能。
/// </summary>
public enum MewSkillFinishReason
{
    Completed,
    Cancelled,
    Failed,
    Destroyed
}

/// <summary>
/// 创建技能时一次性注入的运行上下文。
/// 基础技能通常只需要 Caster、Phase、Player 和 EmitPoint；
/// 二阶段强技能还应传入 ArenaCenter、ArenaRadius、ArenaTransform。
/// </summary>
[Serializable]
public struct MewSkillContext
{
    [NonSerialized] public Empty Caster;
    [NonSerialized] public Transform Player;
    [NonSerialized] public Transform EmitPoint;
    [NonSerialized] public Transform ArenaTransform;

    public MewSkillPhase Phase;
    public Vector2 ArenaCenter;
    public float ArenaRadius;

    public MewSkillContext(
        Empty caster,
        MewSkillPhase phase,
        Transform player,
        Transform emitPoint,
        Vector2 arenaCenter,
        float arenaRadius = 0f,
        Transform arenaTransform = null)
    {
        Caster = caster;
        Phase = phase;
        Player = player;
        EmitPoint = emitPoint;
        ArenaCenter = arenaCenter;
        ArenaRadius = Mathf.Max(0f, arenaRadius);
        ArenaTransform = arenaTransform;
    }

    /// <summary>
    /// 为普通基础技能创建最小上下文。
    /// </summary>
    public static MewSkillContext CreateBasic(
        Empty caster,
        MewSkillPhase phase,
        Transform player,
        Transform emitPoint)
    {
        Vector2 center = caster != null
            ? (Vector2)caster.transform.position
            : emitPoint != null
                ? (Vector2)emitPoint.position
                : Vector2.zero;

        return new MewSkillContext(
            caster,
            phase,
            player,
            emitPoint,
            center);
    }
}

/// <summary>
/// 梦幻技能统一接口。
/// </summary>
public interface IMewSkill
{
    string SkillName { get; }
    float SkillStartup { get; }
    float SkillEndup { get; }
    float ExistTime { get; }
    int Repeat { get; }
    float RepeatTime { get; }

    Empty Caster { get; }
    Transform Player { get; }
    Transform EmitPoint { get; }
    Transform ArenaTransform { get; }
    Vector3 SkillOrigin { get; }
    Vector2 ArenaCenter { get; }
    float ArenaRadius { get; }
    MewSkillPhase Phase { get; }
    MewSkillState State { get; }
    int CurrentRepeatIndex { get; }

    bool IsInitialized { get; }
    bool IsCasting { get; }
    bool IsFinished { get; }

    event Action<IMewSkill, MewSkillFinishReason> Finished;

    /// <summary>
    /// 推荐入口。先完整注入上下文，再决定是否立刻执行。
    /// </summary>
    void Initialize(MewSkillContext context, bool executeImmediately = true);

    /// <summary>
    /// 兼容旧版 Mew.cs 的入口。实现类应自动推断阶段并执行技能。
    /// </summary>
    void SetEmpty(Empty caster);

    void Execute();
    IEnumerator ExecuteWithTime();
    IEnumerator WaitForCompletion();
    void Cancel(bool destroyObject = true);
}

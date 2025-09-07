using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MewBaseSkill : MonoBehaviour, IMewSkill
{
    [SerializeField] protected string skillName;
    [SerializeField] protected float skillStartup;
    [SerializeField] protected float skillEndup;
    [SerializeField] protected float existTime;
    [SerializeField] protected float repeatTime;
    [SerializeField] protected int repeat;

    protected Empty empty;
    protected bool isCasting = false;
    protected float lastCastTime = -1f;
    public string SkillName => skillName;
    public float SkillStartup => skillStartup;
    public float SkillEndup => skillEndup;
    public float ExistTime => existTime;
    public int Repeat => repeat;
    public float RepeatTime => repeatTime;

    void Start()
    {
        Execute();
    }

    public void Execute()
    {
        StartCoroutine(ExecuteWithTime());
    }
    public IEnumerator ExecuteWithTime()
    {
        if (!empty)
        {
            Debug.LogError("技能脚本未设置敌人");
        }

        isCasting = true;
        yield return StartCoroutine(Startup());

        for (int i = 1;i <= repeat; i++)
        {
            yield return StartCoroutine(CoreLogic());
            yield return StartCoroutine(SkillRepeat());
        }

        yield return StartCoroutine(Endup());

        SelfDestory();
        lastCastTime = Time.time;
        isCasting=false;
    }

    public Transform GetPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) return player.transform;
        else return null;
    }

    public Vector2 GetPlayerDirection()
    {
        Transform playerTransform = GetPlayerTransform();
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            return direction;
        }
        else return Vector2.zero;
    }

    public void SelfDestory()
    {
        StopAllCoroutines();
        Destroy(this.gameObject, existTime = 7f);
    }

    public void SetEmpty(Empty empty)
    {
        this.empty = empty;
    }

    //核心逻辑
    public abstract IEnumerator CoreLogic();
    public virtual IEnumerator Startup()
    {
        if(SkillStartup > 0f)
        {
            yield return new WaitForSeconds(SkillStartup);
        }
    }

    public virtual IEnumerator SkillRepeat()
    {
        if(repeatTime > 0f)
        {
            yield return new WaitForSeconds(repeatTime);
        }
    }

    public virtual IEnumerator Endup()
    {
        if (SkillEndup > 0f)
        {
            yield return new WaitForSeconds(SkillEndup);
        }
    }

}
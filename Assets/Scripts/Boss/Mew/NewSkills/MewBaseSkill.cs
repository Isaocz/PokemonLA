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
        yield return new WaitForSeconds(SkillStartup);

        for (int i = 1;i <= repeat; i++)
        {
            StartCoroutine(CoreLogic());
            yield return new WaitForSeconds(repeatTime);
        }

        yield return new WaitForSeconds(SkillEndup);

        SelfDestory();
        lastCastTime = Time.time;
        isCasting=false;
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

}
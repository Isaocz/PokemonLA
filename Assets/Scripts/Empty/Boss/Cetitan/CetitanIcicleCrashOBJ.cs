using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CetitanIcicleCrashOBJ : BreakableEnviroment
{
    public CetitanIcicleCrash ParentIc;

    //×¹ÂäÊÇ·ñÍê³É
    public bool isFallOver = false;


    /// <summary>
    /// ÆÆËéµÄÊ±¼ä
    /// </summary>
    public float BreakTime;


    private void Start()
    {
        StartCoroutine(BeHitbByTime());
    }


    /// <summary>
    /// ËæÊ±¼äÆÆÁÑ
    /// </summary>
    /// <returns></returns>
    IEnumerator BeHitbByTime()
    {
        while (ColliderCount < MaxHP)
        {
            if (isFallOver)
            {
                if (MaxHP - ColliderCount == 1)
                {
                    ParentIc.IcdBreak();
                }
                BeHit(1);
            }

            yield return new WaitForSeconds(2.0f);
        }
    }


    /// <summary>
    /// ´¥Åö±ùÖùÊ±
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (ParentIc != null && ParentIc.ParentCetitan != null)
        {
            if (!isUnBreakable && !isBreak && other.gameObject.tag != "Enviroment" && other.gameObject.tag != "Room" && other.gameObject.tag != "Item" && other.gameObject.tag != "Spike")
            {

                if (other.gameObject.GetInstanceID() == ParentIc.ParentCetitan.gameObject.GetInstanceID())
                {
                    BreakByCetitan();
                }
                else
                {
                    BeHit(GetTouchHitPoint());
                }
            }
        }

    }


    /// <summary>
    ///  ±ùÖùËéÁÑ
    /// </summary>
    public override void Break()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
            transform.parent.GetComponent<Animator>().SetTrigger("Break");

            var Emission1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var Main1 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
            Emission1.enabled = false;
            Main1.loop = false;

            var Emission2 = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
            var Main2 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
            Emission2.enabled = false;
            Main2.loop = false;

            transform.DetachChildren();
            ParentIc.RemoveFromList();
        }
    }

    public void BreakByCetitan()
    {
        BeHit(MaxHP);
        ParentIc.IcdBreak();
    }



    public override int GetHitHitPoint( int Dmage , PokemonType.TypeEnum SkillType)
    {
        float DmageAlpha = Mathf.Clamp((float)(Dmage) / (float)50, 1.0f, 10.0f);
        int TypeAlpha = 1;
        if (SkillType == PokemonType.TypeEnum.Fighting || SkillType == PokemonType.TypeEnum.Rock || SkillType == PokemonType.TypeEnum.Steel || SkillType == PokemonType.TypeEnum.Fire  )
        {
            TypeAlpha = 2;
        }
        int d = (int)(DmageAlpha * TypeAlpha * (float)HitHitPoint);
        Debug.Log(d + "+" + Dmage + "+" + (float)(Dmage) / (float)50 + "+" + DmageAlpha + "+" + TypeAlpha);
        return d;
    }

    private void OnDestroy()
    {
        ParentIc.RemoveFromList();
    }
}

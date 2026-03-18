using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectivirePunchCollider : MonoBehaviour
{
    /// <summary>
    /// ÊÇ·ñÊÇ×óÈ­
    /// </summary>
    public bool isLPunch;


    public Electivire ParentElectivire;

    /// <summary>
    /// À×µçÈ­
    /// </summary>
    public ElectivirePunchEffect ThunderPunch;

    /// <summary>
    /// »ðÑæÈ­
    /// </summary>
    public ElectivirePunchEffect FirePunch;

    /// <summary>
    /// ±ù¶³È­
    /// </summary>
    public ElectivirePunchEffect IcePunch;

    /// <summary>
    /// ¸ñ¶·Ð¡È­
    /// </summary>
    public ElectivirePunchEffect FightPunch;

    /// <summary>
    /// ±¬ÁÑÈ­
    /// </summary>
    public ElectivireDynamicPunchEffect DynamicPunch;

    /// <summary>
    /// Á÷ÐÇÈ­²¨
    /// </summary>
    public ElectivireStarPunchEffect StarPunch;

    /// <summary>
    /// ×·×ÙÁ÷ÐÇÈ­²¨
    /// </summary>
    public ElectivireStarPunchEffect TraceStarPunch;

    /// <summary>
    /// ³¬¼¶ÐîÁ¦È­
    /// </summary>
    public ElectivireDynamicPunchEffect SuperChargePunch;




    ElectivirePunchEffect PunchObj;

    private void Start()
    {
        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), ParentElectivire.GetComponent<Collider2D>(), true);
    }

    private void OnEnable()
    {
        if (PunchObj != null)
        {
            Destroy(PunchObj.gameObject);
        }
        switch (ParentElectivire.NowPunchType)
        {
            case Electivire.PunchType.Ice:
                PunchObj = Instantiate(IcePunch, transform.position, Quaternion.identity, transform.parent);
                break;
            case Electivire.PunchType.Fire:
                PunchObj = Instantiate(FirePunch, transform.position, Quaternion.identity, transform.parent);
                break;
            case Electivire.PunchType.Thunder:
                PunchObj = Instantiate(ThunderPunch, transform.position, Quaternion.identity, transform.parent);
                break;
            case Electivire.PunchType.NormalFight:
                PunchObj = Instantiate(FightPunch, transform.position, Quaternion.identity, transform.parent);
                break;
            case Electivire.PunchType.DynamicPunch:
                ElectivireDynamicPunchEffect DPunchObj = Instantiate(DynamicPunch, transform.position, Quaternion.identity, transform.parent);
                DPunchObj.Collider.ParentElectivire = ParentElectivire;
                PunchObj = DPunchObj;
                break;
            case Electivire.PunchType.StarPunch:
                ElectivireStarPunchEffect SPunchObj = null;
                if (ParentElectivire.IsSuperAngryState)
                {
                    SPunchObj = Instantiate(TraceStarPunch, transform.position, Quaternion.identity, transform.parent);
                }
                else
                {
                    SPunchObj = Instantiate(StarPunch, transform.position, Quaternion.identity, transform.parent);
                }
                ParentElectivire.LunchStarPunch(SPunchObj.StartPunch);
                SPunchObj.transform.parent = null;
                //PunchObj = SPunchObj;
                break;
            case Electivire.PunchType.SuperChargePunch:
                ElectivireDynamicPunchEffect SCPunchObj = Instantiate(SuperChargePunch, transform.position, Quaternion.identity, transform.parent);
                SCPunchObj.Collider.ParentElectivire = ParentElectivire;
                PunchObj = SCPunchObj;
                break;
        }
        if (PunchObj != null) { PunchObj.isLPunch = isLPunch; }
        
    }

    public void OnDisable()
    {
        if (PunchObj != null)
        {
            PunchObj.transform.parent = null;
            PunchObj.PunchOver();
            //Debug.Log(PunchObj.transform.parent);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == ("Player"))
        {
            ParentElectivire.CollisionPlayer(other);
        }
        if (other.transform.tag == "Room" || other.transform.tag == "Enviroment")
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), other.transform.GetComponent<Collider2D>(), true);
        }
    }

}

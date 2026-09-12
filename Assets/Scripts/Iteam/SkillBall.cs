using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBall : IteamPickUp
{
    public Skill GetSkill;
    bool isPickUp;
    Animator animator;
    bool isOpen;
    GameObject SkillMachineItemObj;
    bool isThereAreSkillMechine;
    PlayerControler playerControler;
    public Sprite[] SkillMachineSprite;

    /// <summary>
    /// 精灵球是否空了
    /// </summary>
    public bool isEmpty;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        SkillMachineItemObj = transform.GetChild(3).gameObject;
    }

    private void FixedUpdate()
    {
        if (!CanBePickUp)
        {
            if (isLunch)
            {
                LunchItem();
            }
            else
            {
                DoNotLunch();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isOpen && isPickUp && CanBePickUp)
        {
            //PassiveItemObj.GetComponent<SpriteRenderer>().sprite = passiveList.SpritesList[PassiveDropIndex];
            animator.SetTrigger("Open");
            isOpen = true;
            isThereAreSkillMechine = true;
            if (GetSkill == null)
            {
                GetSkill = playerControler.playerSkillList.RandomGetASkillMachine();
            }
            SkillMachineItemObj.GetComponent<SpriteRenderer>().sprite = SkillMachineSprite[GetSkill.SkillType - 1];
            //Debug.Log(GetSkill);

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") {
            playerControler = other.gameObject.GetComponent<PlayerControler>();
            if (playerControler != null)
            {
                isPickUp = true;
                if (isThereAreSkillMechine)
                {
                    isEmpty = true;
                    string SkillMachineName = "";
                    if (GetSkill.SkillIndex % 10 < 10) { SkillMachineName += "0"; }
                    SkillMachineName += (GetSkill.SkillIndex).ToString() + (GetSkill.SkillChineseName).ToString() + "";
                    UIGetANewItem.UI.GetANewItem(3, SkillMachineName);
                    isThereAreSkillMechine = false;
                    playerControler.animator.SetTrigger("Happy");
                    playerControler.SetTerablast(GetSkill);
                    playerControler.LearnNewSkillByOtherWay(GetSkill);
                    playerControler.PassiveItemGetUI.GetComponent<Image>().sprite = SkillMachineItemObj.GetComponent<SpriteRenderer>().sprite;

                    Destroy(SkillMachineItemObj);
                }
                //开启时音效
                else
                {
                    PickUpEvent();
                }
            }
        }
        
    }

    /// <summary>
    /// 设置技能
    /// </summary>
    /// <param name="Index"></param>
    public bool SetSkill(int Index)
    {
        int SkillIndex = (Index / 2) * 3 - ((Index % 2 == 0) ? 2 : 0 );
        if (SkillIndex < SetSkillPPUPFalse.Instence.SkillList.Count)
        {
            GetSkill = SetSkillPPUPFalse.Instence.SkillList[SkillIndex];
            return true;
        }
        RandomSetSkill();
        return false;
    }


    /// <summary>
    /// 随机设置全技能池技能
    /// </summary>
    /// <param name="Index"></param>
    public void RandomSetSkill()
    {
        int SkillIndex = Random.Range(0, (SetSkillPPUPFalse.Instence.SkillList.Count / 3));
        SkillIndex = SkillIndex * 3;
        if (SkillIndex < SetSkillPPUPFalse.Instence.SkillList.Count)
        {
            GetSkill = SetSkillPPUPFalse.Instence.SkillList[SkillIndex];
            Debug.Log(SkillIndex);
            if (GetSkill == null) { RandomSetSkill(); }
        }
    }


    /// <summary>
    /// 获取测试技能
    /// </summary>
    /// <param name="Index"></param>
    public bool SetStateTestSkill(TestSkill.StateTestType type)
    {
        int SkillIndex = (int)type;
        if (SkillIndex < SetSkillPPUPFalse.Instence.TestSkillList.Count)
        {
            GetSkill = SetSkillPPUPFalse.Instence.TestSkillList[SkillIndex];
            if (GetSkill == null) { RandomSetSkill(); return false; }
            return true;
        }
        return false;
    }


    /// <summary>
    /// 获取清图测试技能
    /// </summary>
    /// <param name="Index"></param>
    public void SetKillAllSkill()
    {
        GetSkill = SetSkillPPUPFalse.Instence.KillAllSkillList[0];
    }

}

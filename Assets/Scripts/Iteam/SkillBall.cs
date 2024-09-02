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
    /// æ´¡È«Ú «∑Òø’¡À
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
        playerControler = other.gameObject.GetComponent<PlayerControler>();
        if (playerControler != null)
        {
            isPickUp = true;
            if (isThereAreSkillMechine)
            {
                isEmpty = true;
                string SkillMachineName = "°∏";
                if (GetSkill.SkillIndex % 10 < 10) { SkillMachineName += "0"; }
                SkillMachineName += (GetSkill.SkillIndex).ToString() + (GetSkill.SkillChineseName).ToString() + "°π";
                UIGetANewItem.UI.GetANewItem(3, SkillMachineName);
                isThereAreSkillMechine = false;
                playerControler.animator.SetTrigger("Happy");
                playerControler.SetTerablast(GetSkill);
                playerControler.LearnNewSkillByOtherWay(GetSkill);
                playerControler.PassiveItemGetUI.GetComponent<Image>().sprite = SkillMachineItemObj.GetComponent<SpriteRenderer>().sprite;
                Destroy(SkillMachineItemObj);
            }
        }
    }


}

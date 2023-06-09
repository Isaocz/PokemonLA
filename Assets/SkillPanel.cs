using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    public static SkillPanel StaticSkillPanel;

    UIPanleSkillBar uIPanleSkillBar01;
    UIPanleSkillBar uIPanleSkillBar02;
    UIPanleSkillBar uIPanleSkillBar03;
    UIPanleSkillBar uIPanleSkillBar04;

    private void OnEnable()
    {
        StaticSkillPanel = this;
        uIPanleSkillBar01 = transform.GetChild(6).GetComponent<UIPanleSkillBar>();
        uIPanleSkillBar02 = transform.GetChild(7).GetComponent<UIPanleSkillBar>();
        uIPanleSkillBar03 = transform.GetChild(8).GetComponent<UIPanleSkillBar>();
        uIPanleSkillBar04 = transform.GetChild(9).GetComponent<UIPanleSkillBar>();
        PlayerControler player = FindObjectOfType<PlayerControler>();
        if (player.Skill01 != null) { uIPanleSkillBar01.gameObject.SetActive(true); uIPanleSkillBar01.GetSkill_Panle(player.Skill01); }
        else { uIPanleSkillBar01.gameObject.SetActive(false);  }
        if (player.Skill02 != null) { uIPanleSkillBar02.gameObject.SetActive(true); uIPanleSkillBar02.GetSkill_Panle(player.Skill02); }
        else { uIPanleSkillBar02.gameObject.SetActive(false); }
        if (player.Skill03 != null) { uIPanleSkillBar03.gameObject.SetActive(true); uIPanleSkillBar03.GetSkill_Panle(player.Skill03); }
        else { uIPanleSkillBar03.gameObject.SetActive(false); }
        if (player.Skill04 != null) { uIPanleSkillBar04.gameObject.SetActive(true); uIPanleSkillBar04.GetSkill_Panle(player.Skill04); }
        else { uIPanleSkillBar04.gameObject.SetActive(false); }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

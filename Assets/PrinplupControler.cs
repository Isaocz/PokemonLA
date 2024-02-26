using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinplupControler : PlayerControler
{
    // Start is called before the first frame update
    public Skill Pound;
    public Skill Growl;
    public Skill WaterGun;
    public Skill MetalClaw;




    // Start is called before the first frame update
    void Start()
    {

        //һ�ټ�ʱ����1059860
        Exp = new int[] {
            9, 48, 39, 39, 44, 57, 79, 105, 141, 182,
            231, 288, 351, 423, 500, 585, 678, 777, 885, 998,
            1119, 1238, 1383, 1527, 1676, 1833, 1998, 2169, 2349, 2534,
            2727, 2928, 3135, 3351, 3572, 3801, 4038, 4218, 4533, 4790,
            5055, 5328, 5607, 5895, 6188, 6489, 6798, 7113, 7437, 7766,
            8103, 8448, 8799, 9159, 9524, 9897, 10278, 10665, 11061, 11462,
            11871, 12288, 12711, 13143, 13580, 14025, 14478, 14937, 15405, 15878,
            16359, 16848, 17343, 17847, 18356, 18873, 19398, 19929, 20469, 21014,
            21567, 22128, 22695, 23271, 23852, 24441, 25038, 25641, 26253, 26870,
            27495, 28128, 28767, 29415, 30068, 30729, 31398, 32073, 32757 };


        if (PlayerAbility == 0) { PlayerAbility = PlayerAbilityList.����; }
        PlayerType01 = 11;
        PlayerType02 = 0;

        Instance();
        InstanceNewSkillPanel();


        if (!isNeedInherit)
        {
            Skill01 = Pound;
            Skill02 = Growl;
            Skill03 = WaterGun;
            Skill04 = MetalClaw;
        }

        skillBar01.GetSkill(Skill01);
        skillBar02.GetSkill(Skill02);
        skillBar03.GetSkill(Skill03);
        skillBar04.GetSkill(Skill04);
        Invoke("LearMetalClawg", 0.01f);

        JudgeEvolutionForEachLevel = NotJudgeEvolution;
    }

    void LearMetalClawg()
    {
        //��������צIndex
        if ((Skill01 == null || Skill01.SkillIndex != 383 && Skill01.SkillIndex != 384) && (Skill02 == null || Skill02.SkillIndex != 383 && Skill02.SkillIndex != 384) && (Skill03 == null || Skill03.SkillIndex != 383 && Skill03.SkillIndex != 384) && (Skill04 == null || Skill04.SkillIndex != 383 && Skill04.SkillIndex != 384))
        {
            LearnNewSkillByOtherWay(MetalClaw);
        }
    }



    private void Update()
    {
        UpdatePlayer();
        StateMaterialChange();
        if (!isEvolution && LevelForSkill >= 36)
        {
            EvolutionStart();
        }
    }

    private void FixedUpdate()
    {
        FixedUpdatePlayer();
    }


}

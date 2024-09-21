using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SylveonControler : PlayerControler
{
    public Skill Tackle;
    public Skill Growl;
    public Skill SandAttack;
    public Skill DisarmingVoice;
    

    // Start is called before the first frame update
    void Start()
    {

        //一百级时经验1000000
        Exp = new int[] {
            8, 19, 37, 61, 91, 127, 169, 217, 271, 331,
            397, 469, 547, 631, 721, 817, 919, 1027, 1141, 1261,
            1387, 1519, 1657, 1801, 1951, 2107, 2269, 2437, 2611, 2791,
            2977, 3169, 3367, 3571, 3781, 3997, 4219, 4447, 4681, 4921,
            5167, 5419, 5677, 5941, 6211, 6487, 6769, 7057, 7351, 7651,
            7957, 8269, 8587, 8911, 9241, 9577, 9919, 10267, 10621, 10981,
            11347, 11719, 12097, 12481, 12871, 13267, 13669, 14077, 14491, 14911,
            15337, 15769, 16207, 16651, 17101, 17557, 18019, 18487, 18961, 19441,
            19927, 20419, 20917, 21421, 21931, 22447, 22969, 23497, 24031, 24571,
            25117, 25669, 26227, 26791, 27361, 27937, 28519, 29107, 29701 };

        PlayerType01 = 18;
        PlayerType02 = 0;
        Instance();
        InstanceNewSkillPanel();



        skillBar01.GetSkill(Skill01);
        skillBar02.GetSkill(Skill02);
        skillBar03.GetSkill(Skill03);
        skillBar04.GetSkill(Skill04);


        Invoke("LearDisarmingVoice", 0.01f);

        JudgeEvolutionForEachLevel = NotJudgeEvolution;
    }

    void LearDisarmingVoice()
    {
        if ((Skill01 == null || Skill01.SkillIndex != 325 && Skill01.SkillIndex != 326) && (Skill02 == null || Skill02.SkillIndex != 325 && Skill02.SkillIndex != 326) && (Skill03 == null || Skill03.SkillIndex != 325 && Skill03.SkillIndex != 326) && (Skill04 == null || Skill04.SkillIndex != 325 && Skill04.SkillIndex != 326))
        {
            LearnNewSkillByOtherWay(DisarmingVoice);
        }
    }

    private void Update()
    {
        UpdatePlayer();
        StateMaterialChange();
        /*
        if (!isEvolution && LevelForSkill >= 5)
        {
            EvolutionStart();
        }
        */
    }

    private void FixedUpdate()
    {
        FixedUpdatePlayer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EeveeControler : PlayerControler
{
    public Skill Tackle;
    public Skill Growl;
    public Skill SandAttack;
    public Skill TestSkill01;

    

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

        if (PlayerAbility == 0) { PlayerAbility = PlayerAbilityList.逃跑; }

        PlayerType01 = 1;
        PlayerType02 = 0;
        Instance();
        InstanceNewSkillPanel();

        Skill01 = Tackle;
        Skill02 = Growl;
        Skill03 = SandAttack;
        Skill04 = TestSkill01;

        skillBar01.GetSkill(Skill01);
        skillBar02.GetSkill(Skill02);
        skillBar03.GetSkill(Skill03);
        skillBar04.GetSkill(Skill04);


        JudgeEvolutionForEachLevel = JudgeSylveonEvolution;
    }

    private void Update()
    {
        UpdatePlayer();
        StateMaterialChange();
        /*
        //测试时为了方便进化，大于五级就进化
        if (!isEvolution && LevelForSkill >= 5)
        {
            EvolutionStart();
        }
        */


    }

    //学会妖精系技能，努力值总和大于40，可以进化为仙子伊布
    bool JudgeSylveonEvolution()
    {
        if (((Skill01 != null && Skill01.SkillType == 18) ||(Skill02 != null && Skill02.SkillType == 18) ||(Skill03 != null && Skill03.SkillType == 18 )|| (Skill04 != null && Skill04.SkillType == 18)) &&
            ((playerData.AtkHardWorkAlways + playerData.DefHardWorkAlways + playerData.HPHardWorkAlways + playerData.LuckHardWorkAlways + playerData.MoveSpeHardWorkAlways + playerData.SpAHardWorkAlways + playerData.SpDHardWorkAlways + playerData.SpeHardWorkAlways) >= 30) )
            { return true; }
        else{ return false; }
    }

    private void FixedUpdate()
    {
        FixedUpdatePlayer();
    }
}

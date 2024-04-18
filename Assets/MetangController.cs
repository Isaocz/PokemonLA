using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetangController : PlayerControler
{
    // Start is called before the first frame update
    public Skill Tackle;
    public Skill BulletPunch;
    public Skill HoneClaws;
    public Skill Confusion;
    public Skill MetalClaw;


    // Start is called before the first frame update
    void Start()
    {

        //一百级时经验1640000
        /*
        Exp = new int[] {
            4, 9, 19, 33, 47, 66, 98, 117, 147, 205,
            222, 263, 361, 366, 500, 589, 686, 794, 914, 1042,
            1184, 1337, 1503, 1681, 1873, 2080, 2299, 2535, 2786, 3051,
            3335, 3634, 3951, 4286, 4639, 3997, 5316, 4536, 6055, 5117,
            6856, 5744, 7721, 6417, 8654, 7136, 9658, 7903, 10734, 8722,
            11883, 9592, 13110, 10515, 14417, 11492, 15805, 12526, 17278, 13616,
            18837, 14766, 20485, 15976, 22224, 17247, 24059, 18581, 25989, 19980,
            28017, 21446, 30146, 22978, 32379, 24580, 34717, 26252, 37165, 27995,
            39722, 29812, 42392, 31704, 45179, 33670, 48083, 35715, 51108, 37839,
            54254, 40043, 57526, 42330, 60925, 44699, 64455, 47153, 68116 };
        */
        //一百级时经验1250000
        Exp = new int[] { 10, 23, 47, 76, 114, 158, 212, 271, 339, 413, 497, 586, 684, 788, 902, 1021, 1149, 1283, 1427, 1576, 1734, 1898, 2072, 2251, 2439, 2633, 2837, 3046, 3264, 3488, 3722, 3961, 4209, 4463, 4727, 4996, 5274, 5558, 5852, 6151, 6459, 6773, 7097, 7426, 7764, 8108, 8462, 8821, 9189, 9563, 9947, 10336, 10734, 11138, 11552, 11971, 12399, 12833, 13277, 13726, 14184, 14648, 15122, 15601, 16089, 16583, 17087, 17596, 18114, 18638, 19172, 19711, 20259, 20813, 21377, 21946, 22524, 23108, 23702, 24301, 24909, 25523, 26147, 26776, 27414, 28058, 28712, 29371, 30039, 30713, 31397, 32086, 32784, 33488, 34202, 34921, 35649, 36383, 37127 };


        if (PlayerAbility == 0) { PlayerAbility = PlayerAbilityList.恒净之躯; }
        PlayerType01 = 9;
        PlayerType02 = 14;

        Instance();
        InstanceNewSkillPanel();

        if (!isNeedInherit)
        {
            Skill01 = Tackle;
            Skill02 = BulletPunch;
            Skill03 = HoneClaws;
            //Skill04 = TestSkill03;
        }


        skillBar01.GetSkill(Skill01);
        skillBar02.GetSkill(Skill02);
        skillBar03.GetSkill(Skill03);
        skillBar04.GetSkill(Skill04);



        JudgeEvolutionForEachLevel = NotJudgeEvolution;

        Invoke("LearConfusion", 0.01f);
        Invoke("LearMetalClaw", 0.23f);

    }

    private void Update()
    {
        UpdatePlayer();
        StateMaterialChange();
        if (!isEvolution && LevelForSkill >= 45)
        {
            EvolutionStart();
        }
    }

    private void FixedUpdate()
    {
        FixedUpdatePlayer();
    }

    void LearConfusion()
    {
        Debug.Log("xxx");
        if ((Skill01 == null || Skill01.SkillIndex != 417 && Skill01.SkillIndex != 418) && (Skill02 == null || Skill02.SkillIndex != 417 && Skill02.SkillIndex != 418) && (Skill03 == null || Skill03.SkillIndex != 417 && Skill03.SkillIndex != 418) && (Skill04 == null || Skill04.SkillIndex != 417 && Skill04.SkillIndex != 418))
        {
            Debug.Log("xxx");
            LearnNewSkillByOtherWay(Confusion);
        }
    }

    void LearMetalClaw()
    {
        if ((Skill01 == null || Skill01.SkillIndex != 383 && Skill01.SkillIndex != 384) && (Skill02 == null || Skill02.SkillIndex != 383 && Skill02.SkillIndex != 384) && (Skill03 == null || Skill03.SkillIndex != 383 && Skill03.SkillIndex != 384) && (Skill04 == null || Skill04.SkillIndex != 383 && Skill04.SkillIndex != 384))
        {
            LearnNewSkillByOtherWay(MetalClaw);
        }
    }



}

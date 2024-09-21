using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiloswineControler : PlayerControler
{

    public Skill Tackle;
    public Skill MudSlup;
    public Skill IceFang;
    public Skill AncientPower;





    // Start is called before the first frame update
    void Start()
    {
        Exp = new int[] { 10, 23, 47, 76, 114, 158, 212, 271, 339, 413, 497, 586, 684, 788, 902, 1021, 1149, 1283, 1427, 1576, 1734, 1898, 2072, 2251, 2439, 2633, 2837, 3046, 3264, 3488, 3722, 3961, 4209, 4463, 4727, 4996, 5274, 5558, 5852, 6151, 6459, 6773, 7097, 7426, 7764, 8108, 8462, 8821, 9189, 9563, 9947, 10336, 10734, 11138, 11552, 11971, 12399, 12833, 13277, 13726, 14184, 14648, 15122, 15601, 16089, 16583, 17087, 17596, 18114, 18638, 19172, 19711, 20259, 20813, 21377, 21946, 22524, 23108, 23702, 24301, 24909, 25523, 26147, 26776, 27414, 28058, 28712, 29371, 30039, 30713, 31397, 32086, 32784, 33488, 34202, 34921, 35649, 36383, 37127 };


        PlayerType01 = 5;
        PlayerType02 = 15;
        Instance();
        InstanceNewSkillPanel();

        skillBar01.GetSkill(Skill01);
        skillBar02.GetSkill(Skill02);
        skillBar03.GetSkill(Skill03);
        skillBar04.GetSkill(Skill04);




        JudgeEvolutionForEachLevel = NotJudgeEvolution;

        

        //GetSkillLevel = new int[] {  8, 20, 23, 25, 10000 };

        Invoke("LearIceFang", 0.01f);

    }

    void LearIceFang()
    {
        if ((Skill01==null || Skill01.SkillIndex != 15 && Skill01.SkillIndex != 16) && ( Skill02==null || Skill02.SkillIndex != 15 && Skill02.SkillIndex != 16) && (Skill03 == null || Skill03.SkillIndex != 15 && Skill03.SkillIndex != 16) && (Skill04 == null || Skill04.SkillIndex != 15 && Skill04.SkillIndex != 16))
        {
            LearnNewSkillByOtherWay(IceFang);
        }
    }


    private void Update()
    {
        UpdatePlayer();
        StateMaterialChange();

    }

    private void FixedUpdate()
    {
        FixedUpdatePlayer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XszControler : PlayerControler
{

    public Skill Tackle;
    public Skill MudSlup;
    //public Skill TestSkill01;
    //public Skill TestSkill02;



    // Start is called before the first frame update
    void Start()
    {
        Exp = new int[] { 10, 23, 47, 76, 114, 158, 212, 271, 339, 413, 497, 586, 684, 788, 902, 1021, 1149, 1283, 1427, 1576, 1734, 1898, 2072, 2251, 2439, 2633, 2837, 3046, 3264, 3488, 3722, 3961, 4209, 4463, 4727, 4996, 5274, 5558, 5852, 6151, 6459, 6773, 7097, 7426, 7764, 8108, 8462, 8821, 9189, 9563, 9947, 10336, 10734, 11138, 11552, 11971, 12399, 12833, 13277, 13726, 14184, 14648, 15122, 15601, 16089, 16583, 17087, 17596, 18114, 18638, 19172, 19711, 20259, 20813, 21377, 21946, 22524, 23108, 23702, 24301, 24909, 25523, 26147, 26776, 27414, 28058, 28712, 29371, 30039, 30713, 31397, 32086, 32784, 33488, 34202, 34921, 35649, 36383, 37127 };



        Instance();
        InstanceNewSkillPanel();

        Skill01 = Tackle;
        Skill02 = MudSlup;
        //Skill03 = TestSkill01;
        //Skill04 = TestSkill02;

        skillBar01.GetSkill(Skill01);
        skillBar02.GetSkill(Skill02);
        skillBar03.GetSkill(Skill03);
        skillBar04.GetSkill(Skill04);
        PlayerType01 = 5;
        PlayerType02 = 15;
        
    }

    private void Update()
    {
        UpdatePlayer();
        StateMaterialChange();
        if(!isEvolution && LevelForSkill >= 16)
        {
            EvolutionStart();
        }
    }

    private void FixedUpdate()
    {
        FixedUpdatePlayer();
    }
}

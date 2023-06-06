using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XszControler : PlayerControler
{

    public Skill Tackle;
    public Skill MudSlup;
    public Skill PowderSnow;
    public Skill Flail;
    public Skill IceShard;
    public Skill Endure;
    public Skill Mist;
    public Skill IcyWind;
    public Skill Earthquake;
    public Skill Amnesia;
    public Skill Blizzard;
    public Skill TakeDown;
    public Skill TeraBlast;

    public Skill Growl;
    public Skill SandAttack;
    

    // Start is called before the first frame update
    void Start()
    {
        
        

        Instance();
        InstanceSkillList();
        InstanceNewSkillPanel();

        Skill01 = Tackle;
        Skill02 = MudSlup;
        Skill03 = SandAttack;
        Skill04 = TeraBlast;

        skillBar01.GetSkill(Skill01);
        skillBar02.GetSkill(Skill02);
        skillBar03.GetSkill(Skill03);
        skillBar04.GetSkill(Skill04);
        PlayerType01 = 5;
        PlayerType02 = 15;




        
        
        GetSkillLevel = new int[] { 9,12,16,20,23,25,10000};
        
    }

    void InstanceSkillList()
    {
        SkillList.Clear();
        //SkillList.Add(MudSlup);
        SkillList.Add(PowderSnow);
        SkillList.Add(Mist);
        SkillList.Add(IceShard);
        SkillList.Add(Endure);
        SkillList.Add(Flail);
        SkillList.Add(IcyWind);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMewSkill
{
    string SkillName { get; }
    float SkillStartup {  get; }
    float SkillEndup {  get; }
    float ExistTime { get; }
    int Repeat {  get; }
    float RepeatTime { get; }
    void Execute();
}

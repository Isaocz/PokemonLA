using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillList : MonoBehaviour
{
    /// <summary>
    /// ����ͨ������ϰ�õļ����б�������Ŀǰ�ļ��ܱ仯
    /// </summary>
    public List<Skill> SkillLearnList;
    /// <summary>
    /// ����ͨ������ϰ�õļ����б�ı��ݣ�����ı�
    /// </summary>
    List<Skill> SkillLearnListBorn = new List<Skill> { };
    /// <summary>
    /// ����б���ʱ��ѡ�����������������ѡһ��Ϊ�˷�ֹ�����������ظ�������������������μ��������ʱ��������
    /// </summary>
    List<Skill> OnceTimeSkillBlackList = new List<Skill> { };
    /// <summary>
    /// ����ͨ������ϰ�õļ��ܵ�Ȩ�شʵ�
    /// </summary>
    public Dictionary<Skill, float> SkillLearnWeightD = new Dictionary<Skill, float>();
    /// <summary>
    /// ѧϰ���ܵĸ��ʱ���Ϊ����ϡ�жȣ���Ϊ�ȼ���1-10����11-20������������91-100����
    /// </summary>
    float[][] LearnSkillPer = new float[][] {
        //          { 1-10��  11-20�� 21-30�� 31-40�� 41-50�� 51-60�� 61-70�� 71-80�� 81-90�� 91-100�� }
        new float[] { 1.0f ,  0.8f ,  0.6f ,  0.5f ,  0.4f ,  0.3f ,  0.3f ,  0.1f ,  0.1f ,  0.1f },//ϡ�ж�1
        new float[] { 0.3f ,  0.7f ,  0.9f ,  0.8f ,  0.7f ,  0.7f ,  0.5f ,  0.4f ,  0.3f ,  0.2f },//ϡ�ж�2
        new float[] { 0.0f ,  0.1f ,  0.6f ,  1.0f ,  1.2f ,  1.0f ,  0.7f ,  0.5f ,  0.3f ,  0.2f },//ϡ�ж�3
        new float[] { 0.0f ,  0.05f ,  0.05f ,  0.1f ,  0.4f ,  0.8f ,  0.8f ,  0.7f ,  0.6f ,  0.5f },//ϡ�ж�4
        new float[] { 0.0f ,  0.0f ,  0.0f ,  0.0f ,  0.1f ,  0.3f ,  0.3f ,  0.4f ,  0.5f ,  0.5f },//ϡ�ж�5
        new float[] { 0.0f ,  0.0f ,  0.0f ,  0.0f ,  0.0f ,  0.1f ,  0.2f ,  0.2f ,  0.3f ,  0.3f } //ϡ�ж�6
    };




    //��ȡ���
    PlayerControler player;
    

    private void Awake()
    {
        player = GetComponent<PlayerControler>();
        for (int i = 0; i < SkillLearnList.Count; i++)
        {
            SkillLearnListBorn.Add(SkillLearnList[i]);
        }
        for (int i = 0; i < SkillMachineList.Count; i++)
        {
            SkillMachineListBorn.Add(SkillMachineList[i]);
        }
        for (int i = 0; i < SkillMewList.Count; i++)
        {
            SkillMewListBorn.Add(SkillMewList[i]);
        }
        AddPlusSkillInList();
    }


    //=================================================ͨ���ȼ�����ѧϰ����===============================================================
    public Skill RandomLearnASkill(int PlayerLevel)
    {
        RemoveNowSkillInList();
        AddPlusSkillInList();

        if ( OnceTimeSkillBlackList.Count >= 3 ) {OnceTimeSkillBlackList.Clear(); }

        SetDictionary(PlayerLevel);
        
        /*
        Debug.Log("xxxxxx");
        foreach (Skill key in SkillLearnWeightD.Keys)
        {
            Debug.Log(key.SkillChineseName + " " + SkillLearnWeightD[key]);
        }
        */
        
        Skill OutPut = null;
        float RanPoint = Random.Range(0, GetTotalWeight(SkillLearnWeightD));
        float counter = 0;

        foreach (var temp in SkillLearnWeightD)
        {
            counter += temp.Value;
            if (RanPoint <= counter)
            {
                OutPut = temp.Key;
                break;
            }
        }


        if (OnceTimeSkillBlackList.Contains(OutPut)) 
        {

            return RandomLearnASkill(PlayerLevel);
        }
        else
        {
            OnceTimeSkillBlackList.Add(OutPut);
            return OutPut;
        }

    }

    void AddPlusSkillInList()
    {
        if (player.Skill01 != null && player.Skill01.PlusSkill != null && player.Skill01.SkillFrom == 0 && !SkillLearnList.Contains(player.Skill01.PlusSkill) && !SkillLearnWeightD.ContainsKey(player.Skill01.PlusSkill))
        {
            SkillLearnList.Add(player.Skill01.PlusSkill);
        }
        if (player.Skill02 != null && player.Skill02.PlusSkill != null && player.Skill02.SkillFrom == 0 && !SkillLearnList.Contains(player.Skill02.PlusSkill) && !SkillLearnWeightD.ContainsKey(player.Skill02.PlusSkill))
        {
            SkillLearnList.Add(player.Skill02.PlusSkill);
        }
        if (player.Skill03 != null && player.Skill03.PlusSkill != null && player.Skill03.SkillFrom == 0 && !SkillLearnList.Contains(player.Skill03.PlusSkill) && !SkillLearnWeightD.ContainsKey(player.Skill03.PlusSkill))
        {
            SkillLearnList.Add(player.Skill03.PlusSkill);
        }
        if (player.Skill04 != null && player.Skill04.PlusSkill != null && player.Skill04.SkillFrom == 0 && !SkillLearnList.Contains(player.Skill04.PlusSkill) && !SkillLearnWeightD.ContainsKey(player.Skill04.PlusSkill))
        {
            SkillLearnList.Add(player.Skill04.PlusSkill);
        }
    }

    void RemoveNowSkillInList()
    {
        for (int i = 0; i<SkillLearnList.Count;)
        {
            if((player.Skill01 != null && SkillLearnList[i].SkillIndex == player.Skill01.SkillIndex) || (player.Skill02 != null && SkillLearnList[i].SkillIndex == player.Skill02.SkillIndex) || (player.Skill03 != null && SkillLearnList[i].SkillIndex == player.Skill03.SkillIndex) || (player.Skill04 != null && SkillLearnList[i].SkillIndex == player.Skill04.SkillIndex)) {
                SkillLearnList.Remove(SkillLearnList[i]);
            }
            else
            {
                i++;
            }
        }
    }

    float GetTotalWeight(Dictionary<Skill, float> SkillD )
    {
        float TotalWeight = 0;
        foreach (var Weight in SkillD.Values)
        {
            TotalWeight += Weight;
        }
        return TotalWeight;
    }

    void SetDictionary(int PlayerLevel)
    {
        SkillLearnWeightD.Clear();
        for (int i = 0 ; i < SkillLearnList.Count ; i++ )
        {
            SkillLearnWeightD[SkillLearnList[i]] = LearnSkillPer[SkillLearnList[i].SkillQualityLevel-1][PlayerLevel/10];
        }
    }

    public void RemoveSkillInList(Skill RemoveSkill , Skill RefreshSkill)
    {
        //Debug.Log(RemoveSkill , RefreshSkill);
        SkillLearnList.Remove(RemoveSkill);
        if (RefreshSkill != null && RefreshSkill.PlusSkill != null && SkillLearnList.Contains(RefreshSkill.PlusSkill))
        {
            SkillLearnList.Remove(RefreshSkill.PlusSkill);
        }
        if (RefreshSkill != null && RefreshSkill.SkillFrom == 2)
        {
            if (SkillLearnListBorn.Contains(RefreshSkill.MinusSkill))
            {
                SkillLearnList.Add(RefreshSkill.MinusSkill);
            }
        }
        if(RefreshSkill != null && SkillLearnListBorn.Contains(RefreshSkill))
        {
            if (RemoveSkill.SkillFrom == 0 || RemoveSkill != RefreshSkill.PlusSkill) {
                SkillLearnList.Add(RefreshSkill);
            }
        }
    }

    //=================================================ͨ���ȼ�����ѧϰ����===============================================================




    //=================================================ͨ������ѧϰ����ȡ����=============================================================

    /// <summary>
    /// ����ѧϰ���ļ����б�
    /// </summary>
    public List<Skill> SkillMachineList;
    /// <summary>
    /// ����ͨ������ϰ�õļ����б�ı��ݣ�����ı�
    /// </summary>
    List<Skill> SkillMachineListBorn = new List<Skill> { };
    /// <summary>
    /// ����ͨ������ϰ�õļ��ܵ�Ȩ�شʵ�
    /// </summary>
    public Dictionary<Skill, float> SkillMachineWeightD = new Dictionary<Skill, float>();
    /// <summary>
    /// ѧϰ���ܻ��ĵȼ�Ʒ�ʸ���
    /// </summary>
    float[] MachineSkillPer = new float[] { 0.5f, 0.7f, 0.7f, 0.6f, 0.2f, 0.1f };




    public Skill RandomGetASkillMachine()
    {
        SetSkillMachineList();
        SetMachineDictionary();

        Skill OutPut = null;
        float RanPoint = Random.Range(0, GetTotalWeight(SkillMachineWeightD));
        float counter = 0;

        foreach (var temp in SkillMachineWeightD)
        {
            counter += temp.Value;
            if (RanPoint <= counter)
            {
                OutPut = temp.Key;
                break;
            }
        }

        return OutPut;
    }

    void SetSkillMachineList()
    {

        SkillMachineList.Clear();
        for (int i = 0; i < SkillMachineListBorn.Count; i++)
        {
            if ((player.Skill01 == null || SkillMachineListBorn[i].SkillIndex != player.Skill01.SkillIndex || SkillMachineListBorn[i].SkillIndex+1 != player.Skill01.SkillIndex) && (player.Skill02 == null || SkillMachineListBorn[i].SkillIndex != player.Skill02.SkillIndex || SkillMachineListBorn[i].SkillIndex+1 != player.Skill02.SkillIndex) && (player.Skill03 == null || SkillMachineListBorn[i].SkillIndex != player.Skill03.SkillIndex || SkillMachineListBorn[i].SkillIndex+1 != player.Skill03.SkillIndex) && (player.Skill04 == null || SkillMachineListBorn[i].SkillIndex != player.Skill04.SkillIndex || SkillMachineListBorn[i].SkillIndex+1 != player.Skill04.SkillIndex))
            {
                SkillMachineList.Add(SkillMachineListBorn[i]);
            }
        }
    }

    void SetMachineDictionary()
    {
        SkillMachineWeightD.Clear();
        for (int i = 0; i < SkillMachineList.Count; i++)
        {
            SkillMachineWeightD[SkillMachineList[i]] = MachineSkillPer[SkillMachineList[i].SkillQualityLevel - 1];
        }
    }


    //=================================================ͨ������ѧϰ����ȡ����=============================================================







    //=================================================���λô�ϰ�ü���=============================================================

    /// <summary>
    /// �λý��ڼ��ܵļ����б�
    /// </summary>
    public List<Skill> SkillMewList;
    /// <summary>
    /// �λý��ڼ��ܵļ����б�ı��ݣ�����ı�
    /// </summary>
    List<Skill> SkillMewListBorn = new List<Skill> { };
    /// <summary>
    /// �λý��ڼ��ܵ�Ȩ�شʵ�
    /// </summary>
    public Dictionary<Skill, float> SkillMewWeightD = new Dictionary<Skill, float>();
    /// <summary>
    /// �λý��ڼ��ܵĵȼ�Ʒ�ʸ���
    /// </summary>
    float[] MewSkillPer = new float[] { 0.5f, 0.7f, 0.7f, 0.6f, 0.2f, 0.1f };

    List<Skill> OnceTimeMewSkillList = new List<Skill> { };




    public Skill RandomGetAMEWSkill()
    {
        SetSkillMewList();
        SetMewDictionary();
        

        
        if (OnceTimeMewSkillList.Count >= 3) { OnceTimeMewSkillList.Clear(); }

        Skill OutPut = null;
        float RanPoint = Random.Range(0, GetTotalWeight(SkillMewWeightD));
        
        float counter = 0;
        
        foreach (var temp in SkillMewWeightD)
        {
            counter += temp.Value;
            if (RanPoint <= counter)
            {
                OutPut = temp.Key;
                break;
            }
        }
        
        if (OnceTimeMewSkillList.Contains(OutPut))
        {
            return RandomGetAMEWSkill();
        }
        else
        {
            OnceTimeMewSkillList.Add(OutPut);
            return OutPut;
        }
        

    }

    void SetSkillMewList()
    {

        SkillMewList.Clear();
        for (int i = 0; i < SkillMewListBorn.Count; i++)
        {
            if ((player.Skill01 == null || SkillMewListBorn[i].SkillIndex != player.Skill01.SkillIndex || SkillMewListBorn[i].SkillIndex+1 != player.Skill01.SkillIndex) && (player.Skill02 == null || SkillMewListBorn[i].SkillIndex != player.Skill02.SkillIndex || SkillMewListBorn[i].SkillIndex+1 != player.Skill02.SkillIndex) && (player.Skill03 == null || SkillMewListBorn[i].SkillIndex != player.Skill03.SkillIndex || SkillMewListBorn[i].SkillIndex+1 != player.Skill03.SkillIndex) && (player.Skill04 == null || SkillMewListBorn[i].SkillIndex != player.Skill04.SkillIndex || SkillMewListBorn[i].SkillIndex+1 != player.Skill04.SkillIndex))
            {
                SkillMewList.Add(SkillMewListBorn[i]);
            }
        }
    }

    void SetMewDictionary()
    {
        SkillMewWeightD.Clear();
        for (int i = 0; i < SkillMewList.Count; i++)
        {
            SkillMewWeightD[SkillMewList[i]] = MewSkillPer[SkillMewList[i].SkillQualityLevel - 1]; 
            
        }
        //Debug.Log(SkillMewListBorn.Count + "+" + SkillMewList.Count + "+" + SkillMewWeightD.Count);
    }

    //=================================================���λô�ϰ�ü���=============================================================

}

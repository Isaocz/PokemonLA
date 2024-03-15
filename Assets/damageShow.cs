using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class damageShow : MonoBehaviour
{
    /// <summary>
    /// 数字显示造成伤害
    /// </summary>
    /// <param name="damage">造成伤害总量</param>
    /// <param name="Crit">是否暴击</param>
    /// <param name="Recover">是否为恢复</param>
    /// <param name="Magic">是否特殊攻击</param>
    public void SetText(int damage, bool Crit, bool Recover, bool Magic, bool ToPlayer = false)
    {
        Transform childTransform = transform.GetChild(0);
        if (childTransform != null)
        {
            Animator animator = childTransform.GetComponent<Animator>();
            if (animator != null)
            {
                if (Crit)
                {
                    animator.SetBool("Crit", true);
                }
            }
            else
            {
                Debug.LogError("子物体没有 Animator 组件！");
            }
        }
        else
        {
            Debug.LogError("没有子物体！");
        }
        int stage = 0;//存储Damage的位数
        int[] num = new int[10];//将每一位进行存储
        while(damage > 0) 
        {
            num[stage] = damage % 10;
            damage = damage / 10;
            stage++;
        }
        string[] nonCritNum = new string[]
        {
            "<sprite=22>",
            "<sprite=23>",
            "<sprite=24>",
            "<sprite=25>",
            "<sprite=26>",
            "<sprite=27>",
            "<sprite=29>",
            "<sprite=30>",
            "<sprite=31>",
            "<sprite=32>"
        };
        string[] CritNum = new string[]
        {
            "<sprite=33>",
            "<sprite=34>",
            "<sprite=35>",
            "<sprite=36>",
            "<sprite=37>",
            "<sprite=38>",
            "<sprite=40>",
            "<sprite=41>",
            "<sprite=42>",
            "<sprite=43>"
        };
        string[] RecoverNum = new string[]
        {
            "<sprite=44>",
            "<sprite=45>",
            "<sprite=46>",
            "<sprite=47>",
            "<sprite=48>",
            "<sprite=49>",
            "<sprite=51>",
            "<sprite=52>",
            "<sprite=53>",
            "<sprite=54>"
        };
        string[] CritRecoverNum = new string[]
        {
            "<sprite=55>",
            "<sprite=56>",
            "<sprite=57>",
            "<sprite=58>",
            "<sprite=59>",
            "<sprite=60>",
            "<sprite=62>",
            "<sprite=63>",
            "<sprite=64>",
            "<sprite=65>"
        };

        string[] MagicNum = new string[]
        {
            "<sprite=99>",
            "<sprite=100>",
            "<sprite=101>",
            "<sprite=102>",
            "<sprite=103>",
            "<sprite=104>",
            "<sprite=106>",
            "<sprite=107>",
            "<sprite=108>",
            "<sprite=109>"
        };

        string[] CritMagicNum = new string[]
        {
            "<sprite=132>",
            "<sprite=133>",
            "<sprite=134>",
            "<sprite=135>",
            "<sprite=136>",
            "<sprite=137>",
            "<sprite=139>",
            "<sprite=140>",
            "<sprite=141>",
            "<sprite=142>"
        };

        string[] HitPlayerNum = new string[]
        {
            "<sprite=166>",
            "<sprite=167>",
            "<sprite=168>",
            "<sprite=169>",
            "<sprite=170>",
            "<sprite=171>",
            "<sprite=173>",
            "<sprite=174>",
            "<sprite=175>",
            "<sprite=176>"
        };

        string[] HealPlayerNum = new string[]
        {
            "<sprite=193>",
            "<sprite=194>",
            "<sprite=195>",
            "<sprite=196>",
            "<sprite=197>",
            "<sprite=198>",
            "<sprite=200>",
            "<sprite=201>",
            "<sprite=202>",
            "<sprite=203>"
        };


        string result = null;
        for(int i = stage - 1; i >= 0; i--)
        {
            switch (Crit, Recover, Magic, ToPlayer)
            {
                case (false, false, false, false): result += nonCritNum[num[i]]; break;
                case (true, false, false, false): result += CritNum[num[i]]; break;
                case (false, true, false, false): result += RecoverNum[num[i]]; break;
                case (true, true, false, false): result += CritRecoverNum[num[i]]; break;
                case (false, false, true, false): result += MagicNum[num[i]]; break;
                case (true, false, true, false): result += CritMagicNum[num[i]]; break;
                case (true , false, false, true):
                case (false, false, false, true): result += HitPlayerNum[num[i]];break;
                case (false, true, false, true): result += HealPlayerNum[num[i]];break;
            }
        }

        transform.GetChild(0).GetComponent<TextMeshPro>().text = result;
        Destroy(gameObject, Crit ? 1.5f : 1f);
    }
}

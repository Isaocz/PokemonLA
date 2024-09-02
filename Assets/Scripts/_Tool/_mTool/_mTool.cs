using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class _mTool : MonoBehaviour
{


    public static float Angle_360(Vector3 from_, Vector3 to_)
    {
        if (from_.x <= 0)
            return Vector3.Angle(from_, to_);
        else
            return 360 - Vector3.Angle(from_, to_);
    }

    public static float Angle_360Y(Vector3 from_, Vector3 to_)
    {
        if (from_.y >= 0)
            return Vector3.Angle(from_, to_);
        else
            return 360 - Vector3.Angle(from_, to_);
    }


    /// <summary>
    /// 检查某个技能的技能标签中是否含有某个标签（如检查某个技能的所有标签中是否含有接触性标签）
    /// </summary>
    public static bool ContainsSkillTag( Skill.SkillTagEnum[] TagList , Skill.SkillTagEnum TargetTag)
    {
        bool OutPut = false;
        foreach (Skill.SkillTagEnum t in TagList)
        {
            if (t == TargetTag) { OutPut = true; break; }
        }
        return OutPut;
    }

    /// <summary>
    /// 输出目标向量的正规方向
    /// </summary>
    /// <returns></returns>
    public static Vector2 MainVector2( Vector2 Input )
    {
        Vector2 OutPut = new Vector2(1,0);
        float a = _mTool.Angle_360Y(Input, Vector2.right);
        if (a >= 45 && a < 135)
        {
            OutPut = new Vector2(0, 1);
        }
        else if (a >= 135 && a < 225)
        {
            OutPut = new Vector2(-1, 0);
        }
        else if (a >= 225 && a < 315)
        {
            OutPut = new Vector2(0, -1);
        }
        else
        {
            OutPut = new Vector2(1, 0);
        }
        return OutPut;

        
    }

    public static string[] Tips = new string[]
    {
        "某一属性的抵抗力每提升一级，受到该属性的伤害就会减少为原本的80%",
        "某一属性的抵抗力每下降一级，受到该属性的伤害就会增加为原本的120%",
        "每个区域都会有精灵中心，友好商店，技能商店",
        "不要忘了在精灵中心查看邮件",
        "烧伤会让攻击下降，中毒会让特攻下降",
        "灵感爆发时会想到新的技能",
        "注意攒钱，不能做小偷！",
        "野生的草丛中可能有遗失的道具，注意检查",
        "打败头目宝可梦获得的奖励可以用于在技能商店学习技能",
        "一次性道具在使用后会消失",
        "携带被动道具后就会自动触发这个被动道具的效果",
        "能力变化和属性抵抗力可以在菜单界面查看",
        "被动道具的效果可以在背包界面查看",
        "离开当前区域前往下一区域时注意看路牌！以便准备下一区域需要的道具",
        "同属性招式间存在一些连携效果",
        "有些适合植物生长的区域会出现果树，注意检查",
        "注意地菱和毒菱",
        "大体型的宝可梦可以直接踏扁一些障碍物，但是也会变得不可避免地无法通过一些陷阱",
        "小体型的宝可梦可以轻松的通过复杂的迷宫，但是对巨大的障碍物毫无办法",
        "可以在精灵中心的杂志架处查看更多提示",
        "可以使用零花钱刷新友好商店的商品，但是会越来越贵，商品也会越来越少",
        "因为太晶能量稀疏，本地区的太晶化与其他地区不太相同。本地区太晶化的效果更像是获得了一个额外的属性。",
        "头目宝可梦有强大的生命力，在受到过高伤害时可以抵御一部分",
        "可以在Skill界面通过拖动技能改变技能的顺序，搭配出更易于操作的组合",
        "不要在梦幻旁边释放技能，否则后果自负！",
        "如果被障碍物困住，可以在原地等待，使用紧急逃脱按钮脱离困境"
    };


    public static void SetSeed()
    {
        if (InitializePlayerSetting.GlobalPlayerSetting != null) { Random.InitState(InitializePlayerSetting.GlobalPlayerSetting.RoundSeed); }
    }

    [DllImport("user32.dll", EntryPoint = "keybd_event")]

    public static extern void Keybd_event(
        byte bvk,//虚拟键值 ESC键对应的是27
        byte bScan,//0
        int dwFlags,//0为按下，1按住，2释放
        int dwExtraInfo//
    );


    /// <summary>
    /// 获得某一Transform的最后一个未被禁用的孙对象
    /// </summary>
    /// <param name="Parent"></param>
    /// <returns></returns>
    public static Transform GetLastGrandChild(Transform Parent)
    {
        Transform Output = GetLastChild(Parent);
        int Count = 0;
        while (Output.childCount != 0)
        {
            Output = GetLastChild(Output);
            Count++;
            if (Count >= 100) { break; }
        }
        return Output;
    }

    /// <summary>
    /// 获得某一Transform的最后一个未被禁用的子对象
    /// </summary>
    /// <param name="Parent"></param>
    /// <returns></returns>
    public static Transform GetLastChild(Transform Parent)
    {
        if (Parent.childCount != 0)
        {
            Transform Output = Parent.GetChild(Parent.childCount - 1);
            int Count = 1;
            while (!Output.gameObject.activeInHierarchy)
            {
                Count++;
                Output = Parent.GetChild(Parent.childCount - Count);
                if (Count >= 100) { break; }
            }
            return Output;
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 能力随提升段衰减的函数，可用于其他衰减 , isClearBody为是否有特性恒净之躯，有的话减算幅度降低
    /// </summary>
    /// <param name="Level"></param>
    /// <returns></returns>
    public static float AbllityChangeFunction(int Level , bool isClearBody = false )
    {
        float Output = 1.0f;
        int AbllityLevel = Mathf.Clamp(Level , -30 , 30);
        if (AbllityLevel >= 0) { Output = Mathf.Pow(Mathf.Log10(4.0f * (float)AbllityLevel + 1.0f), 2.85f) + 1.0f; }
        else            { Output = Mathf.Pow((isClearBody?1.1f: 1.3f) , AbllityLevel); }

        return Output;
    }

}

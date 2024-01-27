using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    };


}

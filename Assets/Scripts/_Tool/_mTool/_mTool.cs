using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

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

    public static Vector2 TiltMainVector2(Vector2 Input)
    {
        return new Vector2(((Input.x < 0) ? -1 : 1), ((Input.y < 0) ? -1 : 1));
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
                if (Count >= 100 || Parent.childCount - Count < 0) { break; }
                Output = Parent.GetChild(Parent.childCount - Count);
            }
            return Output;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取某一个Transform在其夫Transform中的序列号（不包含被禁用对象） 无父对象时返回-1
    /// </summary>
    /// <param name="Child"></param>
    /// <returns></returns>
    public static int GetChildIndex(Transform Child)
    {
        int output = -1;
        //无父对象时
        if (Child.transform.parent == null || !Child.gameObject.activeInHierarchy) { return output; }
        //有父对象时
        else
        {
            //获取父对象
            Transform Parent = Child.transform.parent;
            //检查所有非禁用子对象
            for (int i = 0; i < Parent.childCount; i++)
            {
                if (Parent.GetChild(i).gameObject.activeInHierarchy)
                {
                    output++;
                    if (Parent.GetChild(i) == Child) { break; }
                }
            }
            return output;
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

    /// <summary>
    /// 删除某一对象所有的子对象
    /// </summary>
    public static void RemoveAllChild(GameObject Parent)
    {
        if (Parent.transform.childCount != 0) {
            for (int i = 0; i < Parent.transform.childCount; i++)
            {
                //Debug.Log("Destroy");
                Destroy(Parent.transform.GetChild(i).gameObject);
            }
        }
    }


    /// <summary>
    /// 解除某一对象旗下所有的粒子特效，将这些粒子特效放置于最外场景，并且使他们不再循环并播放完毕后删除
    /// </summary>
    public static void RemoveAllPSChild(GameObject Parent)
    {
        if (Parent.transform.childCount != 0)
        {
            for (int i = 0; i < Parent.transform.childCount; i++)
            {
                if (Parent.transform.GetChild(i).gameObject != null) {
                    //移除孙对象的粒子效果
                    if (Parent.transform.childCount !=  0) {
                        RemoveAllPSChild(Parent.transform.GetChild(i).gameObject);
                    }
                    ParticleSystem ps = Parent.transform.GetChild(i).GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        var psmain = ps.main;
                        psmain.loop = false;
                        psmain.stopAction = ParticleSystemStopAction.Destroy;
                        ps.transform.parent = null;
                    }
                }
            }
        }
    }



    /// <summary>
    /// 获取某个Transform旗下的的所有T
    /// </summary>
    /// <param name="Father"></param>
    /// <returns></returns>
    public static List<T> GetAllFromTransform<T>(Transform Parent)
    {
        List<T> Output = new List<T> { };
        if (Parent.childCount != 0)
        {
            for (int i = 0; i < Parent.childCount; i++)
            {
                T t = Parent.GetChild(i).GetComponent<T>();
                if (t != null) { Output.Add(t); }
                Output = Output.Union(GetAllFromTransform<T>(Parent.GetChild(i))).ToList();
            }
        }
        return Output;
    }


    /// <summary>
    /// 获取某个Transform第一层子对象中的T
    /// </summary>
    /// <param name="Father"></param>
    /// <returns></returns>
    public static List<T> GetAll<T>(Transform Parent)
    {
        List<T> Output = new List<T> { };
        if (Parent.childCount != 0)
        {
            for (int i = 0; i < Parent.childCount; i++)
            {
                T t = Parent.GetChild(i).GetComponent<T>();
                if (t != null) { Output.Add(t); }
            }
        }
        return Output;
    }


    /// <summary>
    /// 给字符串的每个字符间插入一个空格
    /// </summary>
    public static string AddSpaceInString( string s )
    {
        StringBuilder spacedString = new StringBuilder();

        foreach (char c in s)
        {
            spacedString.Append(c).Append(' ');
        }

        // 移除最后一个多余的空格
        if (spacedString.Length > 0)
        {
            spacedString.Length--;
        }

        string result = spacedString.ToString();
        return result;
    }

    /// <summary>
    /// 输出一个list
    /// </summary>
    /// <param name="logList"></param>
    public static void DebugLogList<T>(List<T> logList)
    {
        Debug.Log(string.Join("," , logList ));
    }


    /// <summary>
    /// 排空某个List
    /// </summary>
    public static void RemoveNullInList<T>(List<T> list) where T : UnityEngine.Object
    {
        list.RemoveAll(item => item == null);
    }







    /// <summary>
    /// 忽略某个特定碰撞箱的射线检测
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="dir"></param>
    /// <param name="distance"></param>
    /// <param name="layerMask"></param>
    /// <param name="ignore"></param>
    /// <returns></returns>
    public static RaycastHit2D RaycastIgnoreCollider(Vector2 origin, Vector2 dir, float distance, int layerMask, List<Collider2D> ignore)
    {
        var hits = Physics2D.RaycastAll(origin, dir, distance, layerMask);

        RaycastHit2D closest = default;
        float minDist = float.MaxValue;

        foreach (var h in hits)
        {
            if (ignore.Contains(h.collider))
                continue; // 忽略指定 collider

            if (h.distance < minDist)
            {
                minDist = h.distance;
                closest = h;
            }
        }

        return closest;
    }





}

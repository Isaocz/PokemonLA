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
    /// ���ĳ�����ܵļ��ܱ�ǩ���Ƿ���ĳ����ǩ������ĳ�����ܵ����б�ǩ���Ƿ��нӴ��Ա�ǩ��
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
    /// ���Ŀ�����������淽��
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
        "ĳһ���Եĵֿ���ÿ����һ�����ܵ������Ե��˺��ͻ����Ϊԭ����80%",
        "ĳһ���Եĵֿ���ÿ�½�һ�����ܵ������Ե��˺��ͻ�����Ϊԭ����120%",
        "ÿ�����򶼻��о������ģ��Ѻ��̵꣬�����̵�",
        "��Ҫ�����ھ������Ĳ鿴�ʼ�",
        "���˻��ù����½����ж������ع��½�",
        "��б���ʱ���뵽�µļ���",
        "ע����Ǯ��������С͵��",
        "Ұ���Ĳݴ��п�������ʧ�ĵ��ߣ�ע����",
        "���ͷĿ�����λ�õĽ������������ڼ����̵�ѧϰ����",
        "һ���Ե�����ʹ�ú����ʧ",
        "Я���������ߺ�ͻ��Զ���������������ߵ�Ч��",
        "�����仯�����Եֿ��������ڲ˵�����鿴",
        "�������ߵ�Ч�������ڱ�������鿴",
        "�뿪��ǰ����ǰ����һ����ʱע�⿴·�ƣ��Ա�׼����һ������Ҫ�ĵ���",
        "ͬ������ʽ�����һЩ��ЯЧ��",
        "��Щ�ʺ�ֲ���������������ֹ�����ע����",
        "ע�����Ͷ���",
        "�����͵ı����ο���ֱ��̤��һЩ�ϰ������Ҳ���ò��ɱ�����޷�ͨ��һЩ����",
        "С���͵ı����ο������ɵ�ͨ�����ӵ��Թ������ǶԾ޴���ϰ�����ް취",
        "�����ھ������ĵ���־�ܴ��鿴������ʾ",
        "����ʹ���㻨Ǯˢ���Ѻ��̵����Ʒ�����ǻ�Խ��Խ����ƷҲ��Խ��Խ��",
        "��Ϊ̫������ϡ�裬��������̫����������������̫��ͬ��������̫������Ч�������ǻ����һ����������ԡ�",
        "ͷĿ��������ǿ��������������ܵ������˺�ʱ���Ե���һ����",
        "������Skill����ͨ���϶����ܸı似�ܵ�˳�򣬴���������ڲ��������",
        "��Ҫ���λ��Ա��ͷż��ܣ��������Ը���"
    };


    [DllImport("user32.dll", EntryPoint = "keybd_event")]

    public static extern void Keybd_event(
        byte bvk,//�����ֵ ESC����Ӧ����27
        byte bScan,//0
        int dwFlags,//0Ϊ���£�1��ס��2�ͷ�
        int dwExtraInfo//
    );

}

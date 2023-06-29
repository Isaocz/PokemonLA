using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    /// <summary>
    /// �򵥵�����ʱ��, ����ָ��ÿ��ͣ��ʱִ�еĶ���
    /// </summary>
    /// <param name="duration">ͣ�ٵļ��</param>
    /// <param name="callback">ͣ��ʱ���õĺ���</param>
    /// <returns></returns>
    public static IEnumerator Start(float duration, Action callback)
    {
        return Start(duration, false, callback);
    }


    /// <summary>
    /// �򵥵�����ʱ��, ����ָ��ÿ��ͣ��ʱִ�еĶ���
    /// </summary>
    /// <param name="duration">ÿ��ͣ�ٵļ��</param>
    /// <param name="repeat">�Ƿ��ظ�ִ��</param>
    /// <param name="callback">ͣ��ʱ���õĺ���</param>
    /// <returns></returns>
    public static IEnumerator Start(float duration, bool repeat, Action callback)
    {
        do
        {
            yield return new WaitForSeconds(duration);

            if (callback != null)
                callback();

        } while (repeat);
    }

    public static IEnumerator StartRealtime(float time, System.Action callback)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }

        if (callback != null) callback();
    }

    public static IEnumerator NextFrame(Action callback)
    {
        yield return new WaitForEndOfFrame();

        if (callback != null)
            callback();
    }

    public static IEnumerator RunPerFrame(Action callback)
    {
        do
        {
            yield return new WaitForEndOfFrame();

            if (callback != null)
                callback();

        } while (true);
    }
}

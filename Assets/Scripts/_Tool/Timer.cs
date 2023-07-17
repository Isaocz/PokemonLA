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
    /// �÷� ~
    /// ����:
    /// timer = Timer.Create(1f, () =>
    /// {
    /// });
    /// StartCoroutine(timer);
    /// ֹͣ:
    /// StopCoroutine(timer);
    public static IEnumerator Create(float duration, Action callback)
    {
        return Create(duration, false, callback);
    }



    /// <summary>
    /// �򵥵�����ʱ��, ����ָ��ÿ��ͣ��ʱִ�еĶ���
    /// </summary>
    /// <param name="duration">ÿ��ͣ�ٵļ��</param>
    /// <param name="repeat">�Ƿ��ظ�ִ��</param>
    /// <param name="callback">ͣ��ʱ���õĺ���</param>
    /// <returns></returns>
    public static IEnumerator Create(float duration, bool repeat, Action callback)
    {
        do
        {
            yield return new WaitForSeconds(duration);

            if (callback != null)
                callback();

        } while (repeat);
    }

    /// <summary>
    /// ֧���� callback �ķ���ֵ��̬�ı䶨ʱ�� duration
    /// timer = Timer.Create(1f, () =>
    /// {
    ///     return 0.1f;
    /// });
    /// StartCoroutine(timer);
    public static IEnumerator Create(float duration, Func<float> callback)
    {
        do
        {
            yield return new WaitForSeconds(duration);
            duration = callback();
        } while (true);
    }

    public static IEnumerator Start(MonoBehaviour obj, float duration, Action callback)
    {
        return Start(obj, duration, false, callback); ;
    }

    public static IEnumerator Start(MonoBehaviour obj, float duration, bool repeat, Action callback)
    {
        IEnumerator timer = Create(duration, repeat, callback);
        obj.StartCoroutine(timer);
        return timer;
    }

    public static IEnumerator Start(MonoBehaviour obj, float duration, Func<float> callback)
    {
        IEnumerator timer = Create(duration, callback);
        obj.StartCoroutine(timer);
        return timer;
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

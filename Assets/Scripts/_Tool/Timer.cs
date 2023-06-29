using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    /// <summary>
    /// 简单的正计时器, 可以指定每次停顿时执行的动作
    /// </summary>
    /// <param name="duration">停顿的间隔</param>
    /// <param name="callback">停顿时调用的函数</param>
    /// <returns></returns>
    public static IEnumerator Start(float duration, Action callback)
    {
        return Start(duration, false, callback);
    }


    /// <summary>
    /// 简单的正计时器, 可以指定每次停顿时执行的动作
    /// </summary>
    /// <param name="duration">每次停顿的间隔</param>
    /// <param name="repeat">是否重复执行</param>
    /// <param name="callback">停顿时调用的函数</param>
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

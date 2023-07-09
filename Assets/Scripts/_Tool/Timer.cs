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
    /// 用法 ~
    /// 开启:
    /// timer = Timer.Create(1f, () =>
    /// {
    /// });
    /// StartCoroutine(timer);
    /// 停止:
    /// StopCoroutine(timer);
    public static IEnumerator Create(float duration, Action callback)
    {
        return Create(duration, false, callback);
    }



    /// <summary>
    /// 简单的正计时器, 可以指定每次停顿时执行的动作
    /// </summary>
    /// <param name="duration">每次停顿的间隔</param>
    /// <param name="repeat">是否重复执行</param>
    /// <param name="callback">停顿时调用的函数</param>
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
    /// 支持用 callback 的返回值动态改变定时器 duration
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

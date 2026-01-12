using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGlobalTimer : MonoBehaviour
{
    public static TownGlobalTimer instance { get; private set; }

    public int day;
    public int hour;
    public int minute;
    public float realTimer;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        realTimer += Time.deltaTime;
        if (realTimer > 1f)
        {
            realTimer = 0f;
            AddMinute();
        }
    }

    void AddMinute()
    {
        minute++;
        if(minute >= 60)
        {
            minute = 0;
            AddHour();
        }
    }

    void AddHour()
    {
        hour++;
        if (hour >= 24)
        {
            hour = 0;
            AddDay();
        }
    }

    void AddDay()
    {
        day++;
    }
    /// <summary>
    /// 获取小镇时间
    /// </summary>
    /// <returns>元组（天、小时、分钟）</returns>
    public (int days, int hours, int minutes) GetTownTime()
    {
        return (day, hour, minute);
    }

    /// <summary>
    /// 获取小镇总共运行的时间，也同样是小镇时间，便于比较和计算
    /// </summary>
    /// <param name="DaysInclude">计算是否包含天数</param>
    /// <returns>小镇时间（以分钟计）</returns>
    public int GetMinutes(bool DaysInclude = false)
    {
        if (DaysInclude)
        {
            return (day * 24 * 60) + (hour * 60) + minute;
        }
        else
        {
            return (hour * 60) + minute;
        }
    }

    public void SetTownTime(int days, int hours, int minutes)
    {
        day = days;
        hour = hours;
        minute = minutes;
    }
}

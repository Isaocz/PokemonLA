using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleCrashOBJ : MonoBehaviour
{
    public bool isBreak;
    public int ColliderCount;
    public bool isUnBreakable;


    /// <summary>
    /// 碰撞碎裂的最小次数
    /// </summary>
    public int COUNT_BREAKNEED_MIN = 1;

    /// <summary>
    /// 碰撞碎裂的最大次数
    /// </summary>
    public int COUNT_BREAKNEED_MAX = 6;

    /// <summary>
    /// 破碎的时间
    /// </summary>
    public float BreakTime;

    private void Start()
    {
        if (!isUnBreakable) {
            Invoke("IceBreak", BreakTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isUnBreakable && !isBreak && other.gameObject.tag != "Enviroment" && other.gameObject.tag != "Room" && other.gameObject.tag != "Item" && other.gameObject.tag != "Spike")
        {
            ColliderCount++;
            if (ColliderCount >= Random.Range(COUNT_BREAKNEED_MIN, COUNT_BREAKNEED_MAX))
            {
                IceBreak();
            }
            
        }
    }



    public void IceBreak()
    {
        if (gameObject != null && !isBreak)
        {
            isBreak = true;
            transform.parent.GetComponent<Animator>().SetTrigger("Break");

            var Emission1 = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
            var Main1 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
            Emission1.enabled = false;
            Main1.loop = false;
            
            var Emission2 = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
            var Main2 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
            Emission2.enabled = false;
            Main2.loop = false;

            transform.DetachChildren();
        }
    }



    public void DestorySelf()
    {
        Destroy(gameObject);
    }

}

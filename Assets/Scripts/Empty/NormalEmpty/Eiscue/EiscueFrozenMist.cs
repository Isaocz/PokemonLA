using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EiscueFrozenMist : MonoBehaviour
{
    //冷冻时间和点数
    public float FrozenPoint;
    public float FrozenTime;

    /// <summary>
    /// 冰雾计时器
    /// </summary>
    float Timer;

    /// <summary>
    /// 碰撞箱
    /// </summary>
    Collider2D collider2d;

    // Start is called before the first frame update
    void Start()
    {
        collider2d = GetComponent<Collider2D>();
        collider2d.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if ( !collider2d.enabled && Timer >= 0.8f && Timer < 6.2f)
        {
            collider2d.enabled = true;
            //Debug.Log("True");
        }

        if (collider2d.enabled && Timer >= 6.2f)
        {
            collider2d.enabled = false;
            //Debug.Log("False");
        }
        //Debug.Log(collider2d.enabled);
        if ( Timer >= 10.0f )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerControler p = other.GetComponent<PlayerControler>();
        Debug.Log("++++++++++++++++++++FROZEN+++++++++++++++++++++++");
        if (p != null)
        {
            p.PlayerFrozenFloatPlus(FrozenPoint, FrozenTime);
            Pokemon.PokemonHpChange(null, other.gameObject, 1, 0, 0, PokemonType.TypeEnum.IgnoreType);
            p.KnockOutPoint = 3f;
            p.KnockOutDirection = (p.transform.position - transform.position).normalized;
        }
    }

}

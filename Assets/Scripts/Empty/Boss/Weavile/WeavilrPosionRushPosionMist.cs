using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeavilrPosionRushPosionMist : MonoBehaviour
{


    /// <summary>
    /// ¸¸ÂêáðÀ­
    /// </summary>
    public Weavile ParentWeavile;

    /// <summary>
    /// ¶¾ÎíÅö×²Ê±¼ä
    /// </summary>
    public float Timer_Mist_Collision;

    /// <summary>
    /// ¶¾Îí´æÔÚÊ±¼ä
    /// </summary>
    public float Timer_Mist_Exist;

    /// <summary>
    /// Åö×²Ïä
    /// </summary>
    public Collider2D MistCollider2D;


    public EnemyAudioPlayer audioPlayer;


    private void Start()
    {
        MistCollider2D = GetComponent<Collider2D>();
        Timer.Start(this, Timer_Mist_Collision, ()=> { MistCollider2D.enabled = false;  });
        Timer.Start(this, Timer_Mist_Exist, ()=> { Destroy(gameObject);  });
        if (audioPlayer != null) { audioPlayer.Play(Weavile.WeavileSE.Smoke, transform.position); }
    }

    private void Update()
    {
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            PlayerControler p = collision.GetComponent<PlayerControler>();
            if ( ParentWeavile != null ) { Pokemon.PokemonHpChange(ParentWeavile.transform.gameObject, collision.gameObject, 25, 0, 0, PokemonType.TypeEnum.Poison); }
            else { Pokemon.PokemonHpChange(null, collision.gameObject, 0, 25, 0, PokemonType.TypeEnum.Poison); }
            
            if (p != null)
            {
                p.KnockOutPoint = 3.0f;
                p.KnockOutDirection = (p.transform.position - this.transform.position).normalized;
                p.ToxicFloatPlus(0.2f);
            }
        }
    }
}

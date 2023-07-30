using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryTree : MonoBehaviour
{
    Animator animator;
    public RandomBerryTypeDef Berry;
    public HealthUpCCg CCG;
    public SpaceItem WY;
    public PokemonBall PokemonBall;
    PlayerControler player;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerControler>();
            if (player != null)
            {
                animator.SetTrigger("Drop");
            }
        }
    }

    public void DropABerry()
    {
        float PandomPoint = (player.playerData.IsPassiveGetList[0] ? (Random.Range(0.0f, 1.0f)) : (Random.Range(0.0f, 1.1f)));
        if (PandomPoint <= 0.5f)
        {
            Instantiate(Berry, transform.position, Quaternion.identity, transform).isLunch = true;
        }
        else if(PandomPoint > 0.5f && PandomPoint <= 0.97f)
        {
            Instantiate(CCG, transform.position, Quaternion.identity, transform).isLunch = true;
        }
        else if (PandomPoint > 0.97f && PandomPoint <= 1.0f)
        {
            Instantiate(WY, transform.position, Quaternion.identity, transform).isLunch = true;
        }
    }

}

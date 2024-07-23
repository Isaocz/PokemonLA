using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDropItem : MonoBehaviour
{
    public GameObject MoneyRandom;
    public GameObject Stone;
    public GameObject CandyRandom;
    public GameObject PotionRandom;
    public GameObject HealRandom;
    public GameObject SpaceItemRandom;
    public GameObject TeraShardRandom;
    public GameObject BerryRandom;
    public GameObject XAbllityRandom;
    public GameObject FeatherRandom;
    public GameObject PokemonBall;
    public GameObject SkillItem;
    public GameObject OutPut;



    private void Start()
    {
        float RandomPoint = Random.Range(0.0f, 1.38f);

        if (RandomPoint >= 0.00f && RandomPoint < 0.25f)
        {
            OutPut = Instantiate(MoneyRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.25f && RandomPoint < 0.495f)
        {
            OutPut = Instantiate(Stone, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.495f && RandomPoint < 0.67f)
        {
            OutPut = Instantiate(PotionRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.67f && RandomPoint < 0.75f)
        {
            OutPut = Instantiate(HealRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.75f && RandomPoint < 0.83f)
        {
            OutPut = Instantiate(SpaceItemRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.83f && RandomPoint < 0.86f)
        {
            OutPut = Instantiate(CandyRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.86f && RandomPoint < 0.89f)
        {
            OutPut = Instantiate(TeraShardRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.89f && RandomPoint < 0.94f)
        {
            OutPut = Instantiate(BerryRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.94f && RandomPoint < 1.01f)
        {
            OutPut = Instantiate(XAbllityRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 1.01f && RandomPoint <= 1.05f)
        {
            OutPut = Instantiate(PokemonBall, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 1.05f && RandomPoint <= 1.18f)
        {
            OutPut = Instantiate(FeatherRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 1.18f && RandomPoint <= 1.28f)
        {
            OutPut = Instantiate(SkillItem, transform.position, Quaternion.identity, transform);
        }
        if (OutPut != null)
        {
            OutPut.transform.parent = transform.parent;
        }
        
        Destroy(gameObject);
    }

   
}

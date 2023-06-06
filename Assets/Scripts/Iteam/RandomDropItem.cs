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
    public GameObject OutPut;



    private void Start()
    {
        float RandomPoint = Random.Range(0.0f, 1.13f);

        if (RandomPoint >= 0.00f && RandomPoint < 0.22f)
        {
            OutPut = Instantiate(MoneyRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.22f && RandomPoint < 0.375f)
        {
            OutPut = Instantiate(Stone, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.375f && RandomPoint < 0.53f)
        {
            OutPut = Instantiate(PotionRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.53f && RandomPoint < 0.63f)
        {
            OutPut = Instantiate(HealRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.63f && RandomPoint < 0.71f)
        {
            OutPut = Instantiate(SpaceItemRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.71f && RandomPoint < 0.79f)
        {
            OutPut = Instantiate(CandyRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.79f && RandomPoint < 0.84f)
        {
            OutPut = Instantiate(TeraShardRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.84f && RandomPoint < 0.89f)
        {
            OutPut = Instantiate(BerryRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.89f && RandomPoint < 0.96f)
        {
            OutPut = Instantiate(XAbllityRandom, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 0.96f && RandomPoint <= 1.00f)
        {
            OutPut = Instantiate(PokemonBall, transform.position, Quaternion.identity, transform);
        }
        else if (RandomPoint >= 1.00f && RandomPoint <= 1.13f)
        {
            OutPut = Instantiate(FeatherRandom, transform.position, Quaternion.identity, transform);
        }
        if (OutPut != null)
        {
            OutPut.transform.parent = transform.parent;
        }
        
        Destroy(gameObject);
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WurmpleStringShot : Projectile
{
    public GameObject WurmpleStringShot02;

    private void Awake()
    {
        AwakeProjectile();
    }

    private void Update()
    {
        this.transform.localScale += new Vector3(Time.deltaTime*2, 0, 0);
        DestoryByRange(10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == ("Enviroment") || other.tag == ("Room") || other.tag == ("Player") || (empty.isEmptyInfatuationDone && other.gameObject != empty.gameObject && other.tag == ("Empty")))
        {
            if (other.tag != ("Room"))
            {
                GameObject WSS2 =  Instantiate(WurmpleStringShot02, (transform.position + other.transform.position) / 2, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                WSS2.GetComponent<WurmpleStringShot02>().isProjectelParentInfatuation = empty.isEmptyInfatuationDone;
            }
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStarMoney : MonoBehaviour
{
    public GameObject StartDust;
    public GameObject StartPiece;
    public GameObject OutPut;

    private void Start()
    {
        float RandomPoint = Random.Range(0.0f, 1.0f);
        if (RandomPoint < 0.85f)
        {
            OutPut = Instantiate(StartDust, transform.position, Quaternion.identity, transform);
        }
        else
        {
            OutPut = Instantiate(StartPiece, transform.position, Quaternion.identity, transform);
        }
        OutPut.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}

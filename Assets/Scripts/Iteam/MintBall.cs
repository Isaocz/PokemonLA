using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MintBall : MonoBehaviour
{
    [Tooltip("�������ࣺ1�ӹ��� 2�ӷ��� 3�ӷ��� 4���ط� 5���ٶ� 6���Ӳ���")]
    public int MintType;


    // Start is called before the first frame update
    void Start()
    {
        int output = 20;
        switch (MintType)
        {
            case 1:
                output = Random.Range(0, 4);
                break;
            case 2:
                output = Random.Range(4, 8);
                break;
            case 3:
                output = Random.Range(8, 11);
                break;
            case 4:
                output = Random.Range(12, 16);
                break;
            case 5:
                output = Random.Range(16, 20);
                break;
            case 6:
                output = 20;
                break;
        }
        GetComponent<PokemonBall>().PassiveDropIndex = output;
        GetComponent<PokemonBall>().OpenBall();
    }

}

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
        int output = 54;
        switch (MintType)
        {
            case 1:
                output = Random.Range(34, 38);
                break;
            case 2:
                output = Random.Range(38, 42);
                break;
            case 3:
                output = Random.Range(42, 46);
                break;
            case 4:
                output = Random.Range(46, 50);
                break;
            case 5:
                output = Random.Range(50, 54);
                break;
            case 6:
                output = 54;
                break;
        }
        GetComponent<PokemonBall>().PassiveDropIndex = output;
        GetComponent<PokemonBall>().OpenBall();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MintBall : MonoBehaviour
{
    [Tooltip("薄荷种类：1加攻击 2加防御 3加防御 4加特防 5加速度 6不加不减")]
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

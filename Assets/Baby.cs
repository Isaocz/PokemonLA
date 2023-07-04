using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public int BabySpeciesAtk;
    public int BabySpeciesDef;
    public int BabySpeciesSpA;
    public int BabySpeciesSpD;
    public int BabySpeciesSpe;
    public PlayerControler player;

    public int BabyLevel()
    {
        return player.Level;
    }

    public int BabyAtk()
    {
        return (BabySpeciesAtk * 2 * BabyLevel()) / 100 + 5;
    }

    public int BabyDef()
    {
        return (BabySpeciesDef * 2 * BabyLevel()) / 100 + 5;
    }

    public int BabySpA()
    {
        return (BabySpeciesSpA * 2 * BabyLevel()) / 100 + 5;
    }

    public int BabySpD()
    {
        return (BabySpeciesSpD * 2 * BabyLevel()) / 100 + 5;
    }

    public int BabySpe()
    {
        return (BabySpeciesSpe * 2 * BabyLevel()) / 100 + 5;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }

}

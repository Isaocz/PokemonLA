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
        return (int)((BabySpeciesAtk * 2 * BabyLevel() * ((player.playerData.IsPassiveGetList[87])? 1.5f : 1.0f) ) / 100 + 5);
    }

    public int BabyDef()
    {
        return (int)((BabySpeciesDef * 2 * BabyLevel() * ((player.playerData.IsPassiveGetList[87]) ? 1.5f : 1.0f)) / 100 + 5);
    }

    public int BabySpA()
    {
        return (int)((BabySpeciesSpA * 2 * BabyLevel() * ((player.playerData.IsPassiveGetList[87]) ? 1.5f : 1.0f)) / 100 + 5);
    }

    public int BabySpD()
    {
        return (int)((BabySpeciesSpD * 2 * BabyLevel() * ((player.playerData.IsPassiveGetList[87]) ? 1.5f : 1.0f)) / 100 + 5);
    }

    public int BabySpe()
    {
        return (int)((BabySpeciesSpe * 2 * BabyLevel() * ((player.playerData.IsPassiveGetList[87]) ? 1.5f : 1.0f)) / 100 + 5);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.parent.parent.GetComponent<PlayerControler>();
    }

}

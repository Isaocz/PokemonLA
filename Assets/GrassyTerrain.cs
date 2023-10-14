using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyTerrain : MonoBehaviour
{

    public enum TerrainType
    {
        青草场地,
        精神场地,
        电气场地,
        薄雾场地,
    }

    public bool isSuperMode;
    public TerrainType TType;


    float Timer;

    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 85)
        {
            Destroy(gameObject);

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Empty")
        {
            Pokemon p = other.GetComponent<Pokemon>();
            Substitute s = other.GetComponent<Substitute>();
            if (p != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        p.isInGrassyTerrain = true;
                        if (isSuperMode) { p.isInSuperGrassyTerrain = true; }
                        break;
                    case TerrainType.精神场地:
                        p.isInPsychicTerrain = true;
                        if (isSuperMode) { p.isInSuperPsychicTerrain = true; }
                        break;
                    case TerrainType.电气场地:
                        p.isInElectricTerrain = true;
                        if (isSuperMode) { p.isInSuperElectricTerrain = true; }
                        break;
                    case TerrainType.薄雾场地:
                        p.isInMistyTerrain = true;
                        if (isSuperMode) { p.isInSuperMistyTerrain = true; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        s.isInGrassyTerrain = true;
                        if (isSuperMode) { s.isInSuperGrassyTerrain = true; }
                        break;
                    case TerrainType.精神场地:
                        s.isInPsychicTerrain = true;
                        if (isSuperMode) { s.isInSuperPsychicTerrain = true; }
                        break;
                    case TerrainType.电气场地:
                        s.isInElectricTerrain = true;
                        if (isSuperMode) { s.isInSuperElectricTerrain = true; }
                        break;
                    case TerrainType.薄雾场地:
                        s.isInMistyTerrain = true;
                        if (isSuperMode) { s.isInSuperMistyTerrain = true; }
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Empty")
        {
            Pokemon p = other.GetComponent<Pokemon>();
            Substitute s = other.GetComponent<Substitute>();
            if (p != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        p.isInGrassyTerrain = false;
                        if (isSuperMode) { p.isInSuperGrassyTerrain = false; }
                        break;
                    case TerrainType.精神场地:
                        p.isInPsychicTerrain = false;
                        if (isSuperMode) { p.isInSuperPsychicTerrain = false; }
                        break;
                    case TerrainType.电气场地:
                        p.isInElectricTerrain = false;
                        if (isSuperMode) { p.isInSuperElectricTerrain = false; }
                        break;
                    case TerrainType.薄雾场地:
                        p.isInMistyTerrain = false;
                        if (isSuperMode) { p.isInSuperMistyTerrain = false; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        s.isInGrassyTerrain = false;
                        if (isSuperMode) { s.isInSuperGrassyTerrain = false; }
                        break;
                    case TerrainType.精神场地:
                        s.isInPsychicTerrain = false;
                        if (isSuperMode) { s.isInSuperPsychicTerrain = false; }
                        break;
                    case TerrainType.电气场地:
                        s.isInElectricTerrain = false;
                        if (isSuperMode) { s.isInSuperElectricTerrain = false; }
                        break;
                    case TerrainType.薄雾场地:
                        s.isInMistyTerrain = false;
                        if (isSuperMode) { s.isInSuperMistyTerrain = false; }
                        break;
                }
            }
        }
    }
}

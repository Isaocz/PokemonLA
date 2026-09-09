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
                        p.GrassyTerrainCount += 1;
                        if (isSuperMode) { p.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        p.PsychicTerrainCount += 1;
                        if (isSuperMode) { p.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        p.ElectricTerrainCount += 1;
                        if (isSuperMode) { p.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        p.MistyTerrainCount += 1;
                        if (isSuperMode) { p.SuperMistyTerrainCount += 1; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        p.GrassyTerrainCount += 1;
                        if (isSuperMode) { p.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        p.PsychicTerrainCount += 1;
                        if (isSuperMode) { p.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        p.ElectricTerrainCount += 1;
                        if (isSuperMode) { p.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        p.MistyTerrainCount += 1;
                        if (isSuperMode) { p.SuperMistyTerrainCount += 1; }
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
                        p.GrassyTerrainCount -= 1;
                        if (isSuperMode) { p.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        p.PsychicTerrainCount -= 1;
                        if (isSuperMode) { p.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        p.ElectricTerrainCount -= 1;
                        if (isSuperMode) { p.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        p.MistyTerrainCount -= 1;
                        if (isSuperMode) { p.SuperMistyTerrainCount += 1; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.青草场地:
                        p.GrassyTerrainCount -= 1;
                        if (isSuperMode) { p.SuperGrassyTerrainCount += 1; }
                        break;
                    case TerrainType.精神场地:
                        p.PsychicTerrainCount -= 1;
                        if (isSuperMode) { p.SuperPsychicTerrainCount += 1; }
                        break;
                    case TerrainType.电气场地:
                        p.ElectricTerrainCount -= 1;
                        if (isSuperMode) { p.SuperElectricTerrainCount += 1; }
                        break;
                    case TerrainType.薄雾场地:
                        p.MistyTerrainCount -= 1;
                        if (isSuperMode) { p.SuperMistyTerrainCount += 1; }
                        break;
                }
            }
        }
    }
}

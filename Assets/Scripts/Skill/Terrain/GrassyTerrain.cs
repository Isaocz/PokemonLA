using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassyTerrain : MonoBehaviour
{

    public enum TerrainType
    {
        ��ݳ���,
        ���񳡵�,
        ��������,
        ������,
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
                    case TerrainType.��ݳ���:
                        p.isInGrassyTerrain = true;
                        if (isSuperMode) { p.isInSuperGrassyTerrain = true; }
                        break;
                    case TerrainType.���񳡵�:
                        p.isInPsychicTerrain = true;
                        if (isSuperMode) { p.isInSuperPsychicTerrain = true; }
                        break;
                    case TerrainType.��������:
                        p.isInElectricTerrain = true;
                        if (isSuperMode) { p.isInSuperElectricTerrain = true; }
                        break;
                    case TerrainType.������:
                        p.isInMistyTerrain = true;
                        if (isSuperMode) { p.isInSuperMistyTerrain = true; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.��ݳ���:
                        s.isInGrassyTerrain = true;
                        if (isSuperMode) { s.isInSuperGrassyTerrain = true; }
                        break;
                    case TerrainType.���񳡵�:
                        s.isInPsychicTerrain = true;
                        if (isSuperMode) { s.isInSuperPsychicTerrain = true; }
                        break;
                    case TerrainType.��������:
                        s.isInElectricTerrain = true;
                        if (isSuperMode) { s.isInSuperElectricTerrain = true; }
                        break;
                    case TerrainType.������:
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
                    case TerrainType.��ݳ���:
                        p.isInGrassyTerrain = false;
                        if (isSuperMode) { p.isInSuperGrassyTerrain = false; }
                        break;
                    case TerrainType.���񳡵�:
                        p.isInPsychicTerrain = false;
                        if (isSuperMode) { p.isInSuperPsychicTerrain = false; }
                        break;
                    case TerrainType.��������:
                        p.isInElectricTerrain = false;
                        if (isSuperMode) { p.isInSuperElectricTerrain = false; }
                        break;
                    case TerrainType.������:
                        p.isInMistyTerrain = false;
                        if (isSuperMode) { p.isInSuperMistyTerrain = false; }
                        break;
                }
            }
            if (s != null)
            {
                switch (TType)
                {
                    case TerrainType.��ݳ���:
                        s.isInGrassyTerrain = false;
                        if (isSuperMode) { s.isInSuperGrassyTerrain = false; }
                        break;
                    case TerrainType.���񳡵�:
                        s.isInPsychicTerrain = false;
                        if (isSuperMode) { s.isInSuperPsychicTerrain = false; }
                        break;
                    case TerrainType.��������:
                        s.isInElectricTerrain = false;
                        if (isSuperMode) { s.isInSuperElectricTerrain = false; }
                        break;
                    case TerrainType.������:
                        s.isInMistyTerrain = false;
                        if (isSuperMode) { s.isInSuperMistyTerrain = false; }
                        break;
                }
            }
        }
    }
}

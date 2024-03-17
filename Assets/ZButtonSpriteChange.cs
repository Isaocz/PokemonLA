using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZButtonSpriteChange : MonoBehaviour
{
    public Sprite[] ZButtonSpriteList;

    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = transform.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
        {
            sprite.sprite = ZButtonSpriteList[0];
        }
        else
        {
            switch (InitializePlayerSetting.GlobalPlayerSetting.GetKeybind("Interact"))
            {
                case KeyCode.A:
                    sprite.sprite = ZButtonSpriteList[1];
                    break;
                case KeyCode.B:
                    sprite.sprite = ZButtonSpriteList[2];
                    break;
                case KeyCode.C:
                    sprite.sprite = ZButtonSpriteList[3];
                    break;
                case KeyCode.D:
                    sprite.sprite = ZButtonSpriteList[4];
                    break;
                case KeyCode.E:
                    sprite.sprite = ZButtonSpriteList[5];
                    break;
                case KeyCode.F:
                    sprite.sprite = ZButtonSpriteList[6];
                    break;
                case KeyCode.G:
                    sprite.sprite = ZButtonSpriteList[7];
                    break;
                case KeyCode.H:
                    sprite.sprite = ZButtonSpriteList[8];
                    break;
                case KeyCode.I:
                    sprite.sprite = ZButtonSpriteList[9];
                    break;
                case KeyCode.J:
                    sprite.sprite = ZButtonSpriteList[10];
                    break;



                case KeyCode.K:
                    sprite.sprite = ZButtonSpriteList[11];
                    break;
                case KeyCode.L:
                    sprite.sprite = ZButtonSpriteList[12];
                    break;
                case KeyCode.M:
                    sprite.sprite = ZButtonSpriteList[13];
                    break;
                case KeyCode.N:
                    sprite.sprite = ZButtonSpriteList[14];
                    break;
                case KeyCode.O:
                    sprite.sprite = ZButtonSpriteList[15];
                    break;
                case KeyCode.P:
                    sprite.sprite = ZButtonSpriteList[16];
                    break;
                case KeyCode.Q:
                    sprite.sprite = ZButtonSpriteList[17];
                    break;
                case KeyCode.R:
                    sprite.sprite = ZButtonSpriteList[18];
                    break;
                case KeyCode.S:
                    sprite.sprite = ZButtonSpriteList[19];
                    break;
                case KeyCode.T:
                    sprite.sprite = ZButtonSpriteList[20];
                    break;



                case KeyCode.U:
                    sprite.sprite = ZButtonSpriteList[21];
                    break;
                case KeyCode.V:
                    sprite.sprite = ZButtonSpriteList[22];
                    break;
                case KeyCode.W:
                    sprite.sprite = ZButtonSpriteList[23];
                    break;
                case KeyCode.X:
                    sprite.sprite = ZButtonSpriteList[24];
                    break;
                case KeyCode.Y:
                    sprite.sprite = ZButtonSpriteList[25];
                    break;
                case KeyCode.Z:
                    sprite.sprite = ZButtonSpriteList[26];
                    break;

                default:
                    sprite.sprite = ZButtonSpriteList[0];
                    break;

            }

        }
    }
}

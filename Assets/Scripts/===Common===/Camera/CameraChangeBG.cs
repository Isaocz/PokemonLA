using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeBG : MonoBehaviour
{

    public Sprite BGEevee01R;
    public Sprite BGEevee01U;
    public Sprite BGEevee02R;
    public Sprite BGEevee02U;

    public Sprite BGPika01R;
    public Sprite BGPika01U;
    public Sprite BGPika02R;
    public Sprite BGPika02U;

    public Sprite BGEleM01R;
    public Sprite BGEleM01U;
    public Sprite BGEleM02R;
    public Sprite BGEleM02U;

    public Sprite BGMonster01R;
    public Sprite BGMonster01U;
    public Sprite BGMonster02R;
    public Sprite BGMonster02U;

    public Sprite BGFire01R;
    public Sprite BGFire01U;
    public Sprite BGFire02R;
    public Sprite BGFire02U;

    public Sprite BGWater01R;
    public Sprite BGWater01U;
    public Sprite BGWater02R;
    public Sprite BGWater02U;

    public Sprite BGLeaf01R;
    public Sprite BGLeaf01U;
    public Sprite BGLeaf02R;
    public Sprite BGLeaf02U;

    public Sprite Black01;
    public Sprite Black02;

    CameraAdapt MainCamera;

    private void Start()
    {
        MainCamera = transform.GetComponent<CameraAdapt>();
        if (!PlayerPrefs.HasKey("BackGroundIndex"))
        {
            PlayerPrefs.SetInt("", 0);
            ChangeBG(PlayerPrefs.GetInt("BackGroundIndex"));
        }
        else
        {
            ChangeBG(PlayerPrefs.GetInt("BackGroundIndex"));
        }
    }

    public void ChangeBG(int Index)
    {

        if (MainCamera == null)
        {
            MainCamera = transform.GetComponent<CameraAdapt>();
        }
        switch (Index)
        {
            case 0:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = BGEevee01R;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = BGEevee01R;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = BGEevee01U;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = BGEevee01U;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEevee02R;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEevee02R;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEevee02U;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEevee02U;
                break;
            case 1:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = BGPika01R;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = BGPika01R;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = BGPika01U;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = BGPika01U;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGPika02R;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGPika02R;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGPika02U;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGPika02U;
                break;
            case 2:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = BGEleM01R;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = BGEleM01R;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = BGEleM01U;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = BGEleM01U;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEleM02R;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEleM02R;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEleM02U;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGEleM02U;
                break;
            case 3:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = BGMonster01R;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = BGMonster01R;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = BGMonster01U;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = BGMonster01U;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGMonster02R;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGMonster02R;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGMonster02U;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGMonster02U;
                break;
            case 4:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = BGFire01R;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = BGFire01R;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = BGFire01U;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = BGFire01U;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGFire02R;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGFire02R;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGFire02U;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGFire02U;
                break;
            case 5:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = BGWater01R;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = BGWater01R;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = BGWater01U;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = BGWater01U;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGWater02R;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGWater02R;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGWater02U;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGWater02U;
                break;
            case 6:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = BGLeaf01R;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = BGLeaf01R;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = BGLeaf01U;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = BGLeaf01U;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGLeaf02R;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGLeaf02R;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGLeaf02U;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = BGLeaf02U;
                break;
            case 7:
                MainCamera.cameraMaskLeft.GetComponent<SpriteRenderer>().sprite = Black01;
                MainCamera.cameraMaskRight.GetComponent<SpriteRenderer>().sprite = Black01;
                MainCamera.cameraMaskDown.GetComponent<SpriteRenderer>().sprite = Black02;
                MainCamera.cameraMaskUp.GetComponent<SpriteRenderer>().sprite = Black02;
                MainCamera.cameraMaskLeft.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Black01;
                MainCamera.cameraMaskRight.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Black01;
                MainCamera.cameraMaskDown.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Black02;
                MainCamera.cameraMaskUp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Black02;
                break;

        }
    }
}

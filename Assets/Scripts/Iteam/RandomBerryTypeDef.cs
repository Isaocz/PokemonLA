using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBerryTypeDef : MonoBehaviour
{
    public GameObject SpaceItemList;
    public GameObject OutPut;
    public bool isLunch;
    public bool BanLunchUp;
    public bool BanLunchDown;
    public bool BanLunchRight;
    public bool BanLunchLeft;

    private void Start()
    {
        int RandomPoint = (int)(Random.Range(2.7f, 4.5f)*10);
        OutPut = Instantiate(SpaceItemList.transform.GetChild(RandomPoint), transform.position, Quaternion.identity, transform).gameObject;
        OutPut.transform.parent = transform.parent;
        if (isLunch) { OutPut.GetComponent<IteamPickUp>().isLunch = true; }
        if (BanLunchUp) { OutPut.GetComponent<IteamPickUp>().BanLunchUp = true; }
        if (BanLunchDown) { OutPut.GetComponent<IteamPickUp>().BanLunchDown = true; }
        if (BanLunchRight) { OutPut.GetComponent<IteamPickUp>().BanLunchRight = true; }
        if (BanLunchLeft) { OutPut.GetComponent<IteamPickUp>().BanLunchLeft = true; }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMiniMap : MonoBehaviour
{

    public static UiMiniMap Instance { get; private set; }

    public GameObject mapcreater;
    public Image MiniMapBlock;
    public Image MiniMapPcBlock;
    public Image MiniMapStoreBlock;
    public Image MiniMapBackGround;
    public Image MiniMapMark;
    public List<Image> VisitedRoomList = new List<Image>();

    GameObject MainCamera;
    Image NowMark;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        MainCamera = GameObject.FindWithTag("MainCamera");
    }


    // Start is called before the first frame update
    public void CreatMiniMap()
    {
        MapCreater map = mapcreater.GetComponent<MapCreater>();
        foreach (Vector3Int item in map.VRoom.Keys)
        {
            if (item == map.PCRoomPoint)
            {
                string roomname = item.ToString();
                Image roomblock = Instantiate(MiniMapPcBlock, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
                roomblock.rectTransform.anchoredPosition = new Vector3(item.x * 8.2f, item.y * 8.2f, 0);
                roomblock.color = new Vector4(255, 255, 255, 0);
                roomblock.transform.name = roomname;
            }else if(item == map.StoreRoomPoint)
            {
                string roomname = item.ToString();
                Image roomblock = Instantiate(MiniMapStoreBlock, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
                roomblock.rectTransform.anchoredPosition = new Vector3(item.x * 8.2f, item.y * 8.2f, 0);
                roomblock.color = new Vector4(255, 255, 255, 0);
                roomblock.transform.name = roomname;
            }
            else
            {
                string roomname = item.ToString();
                Image roomblock = Instantiate(MiniMapBlock, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
                roomblock.rectTransform.anchoredPosition = new Vector3(item.x * 8.2f, item.y * 8.2f, 0);
                roomblock.color = new Vector4(255, 255, 255, 0);
                roomblock.transform.name = roomname;
            }
        }
        NowMark = Instantiate(MiniMapMark, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        
        NowMark.rectTransform.anchoredPosition = new Vector3(0, 0, 0);

    }

    public void MiniMapMove(Vector3 direction)
    {
        foreach(Transform i in transform)
        {
            Image I = i.GetComponent<Image>();
            I.rectTransform.anchoredPosition = new Vector3((float)(I.rectTransform.anchoredPosition.x + 8.2 * direction.x), (float)(I.rectTransform.anchoredPosition.y + 8.2 * direction.y), 0);
        }
        NowMark.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //NowMark.rectTransform.anchoredPosition = new Vector3((MainCamera.transform.position.x / 30.0f) * 8.2f, ((MainCamera.transform.position.y - 0.7f) / 24.0f) * 8.2f, 0);
        if (Input.GetKey(KeyCode.Tab))
        {
            MiniMapBackGround.transform.parent.GetComponent<Image>().color = new Vector4(255, 255, 255, 0);
            MiniMapBackGround.color = new Vector4(255,255,255,0);
            MiniMapBackGround.GetComponent<Mask>().enabled = false;
            MiniMapBackGround.rectTransform.localScale = new Vector3(3.4f, 3.4f, 0);
        }
        else
        {
            MiniMapBackGround.transform.parent.GetComponent<Image>().color = new Vector4(255, 255, 255, 255);
            MiniMapBackGround.color = new Vector4(255, 255, 255, 255);
            MiniMapBackGround.GetComponent<Mask>().enabled = true;
            MiniMapBackGround.rectTransform.localScale = new Vector3(1, 1, 0);
        }
    }

    public void SeeMapJustOneRoom()
    {
        foreach (Transform MapCell in transform)
        {
            if(MapCell.GetComponent<Image>() != null && MapCell.GetComponent<Image>().color.a == 0)
            {
                MapCell.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
            }
        }        
    }

    public void SeeMap()
    {
        foreach (Transform MapCell in transform)
        {
            if (MapCell.GetComponent<Image>() != null && MapCell.GetComponent<Image>().color.a == 0)
            {
                MapCell.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 1);
                VisitedRoomList.Add(MapCell.GetComponent<Image>());
            }
        }
    }

    public void SeeMapOver()
    {
        foreach (Transform child in transform)
        {
            if(VisitedRoomList.Exists(t => t == child.GetComponent<Image>()))
            {
                continue;
            }
            else
            {
                child.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0);
                NowMark.color = new Color(1, 1, 1, 1);
            }
        }
    }
}

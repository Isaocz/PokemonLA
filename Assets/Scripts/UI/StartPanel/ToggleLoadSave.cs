using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleLoadSave : MonoBehaviour
{

    public int SaveIndex;

    public Text SaveName;
    public Text LastTime;
    public Text APTotal;
    public Text ACTotal;

    //徽章
    public GameObject[] Badgelist;

    //生成徽章的父对象
    public GameObject BadgeParentTransform;

    public SaveData d;



    // Start is called before the first frame update
    void Start()
    {
        SetToggle();
    }

    public void SetToggle()
    {
        d = SaveSystem.saveSystem.LoadGame(SaveIndex);
        
        if (d == null)
        {
            SaveName.text = "<空存档位>";
            LastTime.gameObject.SetActive(false);
            APTotal.gameObject.SetActive(false);
            ACTotal.gameObject.SetActive(false);
            BadgeParentTransform.gameObject.SetActive(false);
        }
        else
        {
            LastTime.gameObject.SetActive(true);
            APTotal.gameObject.SetActive(true);
            ACTotal.gameObject.SetActive(true);
            BadgeParentTransform.gameObject.SetActive(true);
            SaveName.text = d.SaveName + "冒险团";
            LastTime.text = "（上次游戏时间：" + d.LastGameTime  + ")";
            APTotal.text = "总冒险点数：" + d.APTotal;
            ACTotal.text = "总冒险次数：" + d.GameCount;
            SetLevelBar(d.GroupLevel);
        }
    }


    //根据冒险团等级设置经验条
    public void SetLevelBar(int Level)
    {
        //移除曾经的徽章
        if (BadgeParentTransform.transform.childCount != 0)
        {
            foreach (Transform child in BadgeParentTransform.transform)
            {
                Destroy(child.gameObject);
            }
        }
        Instantiate(Badgelist[Level], BadgeParentTransform.transform.position, Quaternion.identity, BadgeParentTransform.transform).transform.localScale = new Vector3(0.4f,0.4f,1) ;
    }


}

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

    //����
    public GameObject[] Badgelist;

    //���ɻ��µĸ�����
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
            SaveName.text = "<�մ浵λ>";
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
            SaveName.text = d.SaveName + "ð����";
            LastTime.text = "���ϴ���Ϸʱ�䣺" + d.LastGameTime  + ")";
            APTotal.text = "��ð�յ�����" + d.APTotal;
            ACTotal.text = "��ð�մ�����" + d.GameCount;
            SetLevelBar(d.GroupLevel);
        }
    }


    //����ð���ŵȼ����þ�����
    public void SetLevelBar(int Level)
    {
        //�Ƴ������Ļ���
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

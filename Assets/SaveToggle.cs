using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveToggle : MonoBehaviour
{
    //存档选择按钮列表
    List<Toggle> TogglesList = new List<Toggle> { };
    //存档删除按钮列表
    List<Button> DeleteButtonList = new List<Button> { };

    //游戏开始按钮
    public Button GameStartButton;

    //删除存档界面
    public GameObject DeleteSavePanel;

    //新建存档界面
    public GameObject NewSavePanel;

    //当前选择存档的序列号
    public int SaveIndex = 0;

    //当前删除存档的序列号
    public int DeleteIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameStartButton.interactable = false;

        for (int i = 0; i < transform.GetChild(3).childCount; i++)
        {
            Toggle t = transform.GetChild(3).GetChild(i).GetComponent<Toggle>();
            if (t != null) { TogglesList.Add(t); }
        }
    }


    public void SelectSave(int Index)
    {
        Index--;
        if (!TogglesList[Index].isOn) { GameStartButton.interactable = false; SaveIndex = 0; }
        else
        {
            for (int i = 0; i < TogglesList.Count; i++)
            {
                if (i != Index) { TogglesList[i].isOn = false; }
            }
            SaveIndex = Index+ 1;
            if (TogglesList[SaveIndex - 1].GetComponent<ToggleLoadSave>().d == null) 
            {
                NewSavePanel.gameObject.SetActive(true);
            }
            else
            {
                GameStartButton.interactable = true;
            }

        }

    }



    public void CallDeleteSavePanel(int Index )
    {
        if (TogglesList[Index - 1].GetComponent<ToggleLoadSave>().d != null) {
            DeleteSavePanel.gameObject.SetActive(true);
            DeleteSavePanel.transform.GetChild(1).GetComponent<Text>().text = "  确定要删除存档<" + TogglesList[Index - 1].GetComponent<ToggleLoadSave>().d.SaveName + "冒险团>吗\n？摁下[确定]按钮后会永久删除该存档，并且不可恢复！";
            DeleteIndex = Index;
        }
    }

    public void DeleteSaveButton()
    {
        if (DeleteIndex == 1 || DeleteIndex == 2 || DeleteIndex == 3) {
            SaveSystem.saveSystem.DeleteSave(DeleteIndex);
            TogglesList[DeleteIndex-1].GetComponent<ToggleLoadSave>().SetToggle();
            TogglesList[DeleteIndex - 1].isOn = false;
            ResetDeleteIndex();
        }
    }

    public void ResetDeleteIndex() { DeleteIndex = 0; }

    
    public void CreatNewSave()
    {
        if (SaveIndex != 0 && NewSavePanel.transform.GetChild(2).GetComponent<InputField>().text != "")
        {
            if (TogglesList[SaveIndex - 1].GetComponent<ToggleLoadSave>().d == null)
            {
                SaveSystem.saveSystem.NewGame(SaveIndex, NewSavePanel.transform.GetChild(2).GetComponent<InputField>().text);
               
                TogglesList[SaveIndex - 1].GetComponent<ToggleLoadSave>().SetToggle();
                NewSavePanel.SetActive(false);
                GameStartButton.interactable = true;
            }

        }
    }


    public void GameStart()
    {
        if ((SaveIndex == 1 && SaveSystem.saveSystem.ExitSave(1)) || (SaveIndex == 2 && SaveSystem.saveSystem.ExitSave(2)) || (SaveIndex == 3 && SaveSystem.saveSystem.ExitSave(3)))
        {
            SaveLoader.saveLoader.saveData = TogglesList[SaveIndex - 1].GetComponent<ToggleLoadSave>().d;
            SceneLoadManger.sceneLoadManger.ReturnTown();
        }
    }
}

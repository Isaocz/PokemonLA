using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManger : MonoBehaviour
{
    
    public static SceneLoadManger sceneLoadManger;
    public PlayerControler Player;
    public int PlayerAbilityIndex;

    private void Awake()
    {
        sceneLoadManger = this;
        if (FindObjectOfType<MapCreater>() != null && FindObjectOfType<PlayerControler>() == null) {
            Player = StartPanelPlayerData.PlayerData.Player;
            PlayerAbilityIndex = StartPanelPlayerData.PlayerData.PlayerAbilityIndex;
            PlayerControler p = Instantiate(Player, Vector3.zero, Quaternion.identity);
            Debug.Log(Player);
            UIPanelGwtNewSkill.StaticUIGNS.SetPlayer(p);
            switch (PlayerAbilityIndex)
            {
                case 0: p.PlayerAbility = p.playerAbility01; break;
                case 1: p.PlayerAbility = p.playerAbility02; break;
                case 2: p.PlayerAbility = p.playerAbilityDream; break;
            }
            Debug.Log(p);
        }
        if (FindObjectOfType<PlayerControler>() != null) { PlayerControler player = FindObjectOfType<PlayerControler>(); player.transform.position = Vector3.zero; player.NowRoom = Vector3Int.zero; player.InANewRoom = true; player.NewRoomTimer = 0f;
        }
    }

    private void Start()
    {

    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.FloorNumber == -2 ) { FloorNum.GlobalFloorNum.FloorNumber = 0; }
        SceneManager.LoadSceneAsync(2);
    }

    public void ReturnTitle()
    {
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.FloorNumber != -2) { FloorNum.GlobalFloorNum.InstanceFloorNum(); }
        SaveLoader.saveLoader.saveData = null;
        SceneManager.LoadSceneAsync(1);
    }

    public void ReturnTown()
    {
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.FloorNumber != -1) { FloorNum.GlobalFloorNum.InstanceFloorNum(); FloorNum.GlobalFloorNum.FloorNumber = -1; }
        SceneManager.LoadSceneAsync(2);
    }

    public void AdventureOver()
    {
        if (FloorNum.GlobalFloorNum != null && FloorNum.GlobalFloorNum.FloorNumber != -1) { FloorNum.GlobalFloorNum.InstanceFloorNum(); FloorNum.GlobalFloorNum.FloorNumber = -2; }


        SceneManager.LoadSceneAsync(3);
    }

    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        SaveLoader.saveLoader.saveData = null;
        Application.Quit();
    }

    public void SaveGame()
    {
        if (SaveSystem.saveSystem != null && SaveLoader.saveLoader != null) { SaveSystem.saveSystem.SaveGame(SaveLoader.saveLoader.saveData); }
    }
}

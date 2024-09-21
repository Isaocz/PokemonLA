using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 指示当前选择的玩家和性格
/// </summary>
public class StartPanelPlayerData : MonoBehaviour
{

    public static StartPanelPlayerData PlayerData;


    /// <summary>
    /// 选择的角色
    /// </summary>
    public PlayerControler Player;
    /// <summary>
    /// 玩家的初始特性
    /// </summary>
    public int PlayerAbilityIndex;
    /// <summary>
    /// 玩家的初始性格
    /// 第一位是增幅项，第二位是减少项
    /// 0无 1攻击 2防御 3特攻 4特防 5攻速 6随机
    /// 如（0，0）代表不增不减 ，（1，5）代表加攻击减攻速 ， （6，1）代表减攻击增幅项随机
    /// 如果两者相等且不为随机，也代表不增不减（如（1，1），（4，4））
    /// </summary>
    public Vector2Int PlayerNature = new Vector2Int(6,6);
    /// <summary>
    /// 玩家的初始道具
    /// </summary>
    public int PlayerInitialItem = -1;
    /// <summary>
    /// 本局游戏是不是种子局
    /// </summary>
    public bool isSeedGame = false;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        PlayerData = this;
    }

    public void ResetPlayerData()
    {
        PlayerAbilityIndex = -1;
        PlayerNature = new Vector2Int(6, 6);
        PlayerInitialItem = -1;
        isSeedGame = false;
    }
}

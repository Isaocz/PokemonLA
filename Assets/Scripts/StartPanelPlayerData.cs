using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ָʾ��ǰѡ�����Һ��Ը�
/// </summary>
public class StartPanelPlayerData : MonoBehaviour
{

    public static StartPanelPlayerData PlayerData;


    /// <summary>
    /// ѡ��Ľ�ɫ
    /// </summary>
    public PlayerControler Player;
    /// <summary>
    /// ��ҵĳ�ʼ����
    /// </summary>
    public int PlayerAbilityIndex;
    /// <summary>
    /// ��ҵĳ�ʼ�Ը�
    /// ��һλ��������ڶ�λ�Ǽ�����
    /// 0�� 1���� 2���� 3�ع� 4�ط� 5���� 6���
    /// �磨0��0������������ ����1��5������ӹ��������� �� ��6��1��������������������
    /// �����������Ҳ�Ϊ�����Ҳ�������������磨1��1������4��4����
    /// </summary>
    public Vector2Int PlayerNature = new Vector2Int(6,6);
    /// <summary>
    /// ��ҵĳ�ʼ����
    /// </summary>
    public int PlayerInitialItem = -1;
    /// <summary>
    /// ������Ϸ�ǲ������Ӿ�
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

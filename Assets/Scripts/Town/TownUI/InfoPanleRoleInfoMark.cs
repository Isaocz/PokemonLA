using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanleRoleInfoMark : MonoBehaviour
{

    public Image Head;
    public Image Candy;
    public Text CandyCount;

    // Start is called before the first frame update
    public void SetRoleInfo( RoleInfo r )
    {
        CandyCount.text = r.Candy.ToString();
        Head.sprite = r.Role.PlayerHead;
        Candy.sprite = r.Role.PlayerCandy;
    }
}

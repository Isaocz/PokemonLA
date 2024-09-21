using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPlayDoor : MonoBehaviour
{

    public GameObject SelectRolePanel;
    TownPlayer Player;

    private void FixedUpdate()
    {
        if (SelectRolePanel.gameObject.activeInHierarchy && SelectRolePanel != null && Player != null)
        {
            if ((transform.position - Player.transform.position).magnitude > 6.0f) { SelectRolePanel.gameObject.SetActive(false); }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!SelectRolePanel.gameObject.activeInHierarchy && other.tag == "Player")
        {
            Debug.Log("stay");
            if (Player == null) { Player = other.gameObject.GetComponent<TownPlayer>(); }
            if (SelectRolePanel != null && Player != null)
            {
                SelectRolePanel.gameObject.SetActive(true);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (SelectRolePanel.gameObject.activeInHierarchy && other.tag == "Player")
        {
            Debug.Log("exit");
            if (Player == null) { Player = other.gameObject.GetComponent<TownPlayer>(); } 
            if (SelectRolePanel != null && Player != null)
            {
                SelectRolePanel.gameObject.SetActive(false);
            }
        }
    }


}

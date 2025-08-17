using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentToogle : EnviromentSwitch
{

    /// <summary>
    /// 踩住按钮的碰撞箱
    /// </summary>
    public List<GameObject> GameObjectList = new List<GameObject> { };

    /// <summary>
    /// 有人踩住按钮
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Player")
        {
            if (!GameObjectList.Contains(other.gameObject)) { GameObjectList.Add(other.gameObject); }
            if (GameObjectList.Count > 0 && ChangeState == changeState.idle && !isON)
            {
                StartCoroutine(SwitchON());
            }
        }

    }

    /// <summary>
    /// 有人离开按钮
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Player")
        {
            if (GameObjectList.Contains(other.gameObject)) { GameObjectList.Remove(other.gameObject); }
            if (GameObjectList.Count <= 0 && ChangeState == changeState.idle && isON)
            {
                StartCoroutine(SwitchOFF());
            }
        }

    }
}

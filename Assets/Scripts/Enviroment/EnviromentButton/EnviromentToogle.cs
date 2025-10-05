using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentToogle : EnviromentSwitch
{

    /// <summary>
    /// ��ס��ť����ײ��
    /// </summary>
    public List<GameObject> GameObjectList = new List<GameObject> { };

    /// <summary>
    /// ���˲�ס��ť
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
    /// �����뿪��ť
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

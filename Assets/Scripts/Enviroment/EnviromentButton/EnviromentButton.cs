using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentButton : EnviromentSwitch
{
    

    /// <summary>
    /// 碰撞
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Player" )
        {
            //未被改变时 且未被摁下才可以改变
            if (ChangeState == changeState.idle && !isON)
            {
                StartCoroutine(SwitchON());
            }
        }

    }


    /// <summary>
    /// 寻找父房间
    /// </summary>
    /// <returns></returns>
    protected Room FindParentRoom()
    {
        Transform parent = transform.parent;
        int count = 0;

        while (parent != null && count < 10)
        {
            Room r = parent.GetComponent<Room>();
            if (r != null)
            {
                return r;
            }

            parent = parent.parent;
            count++;
        }

        return null;
    }

}

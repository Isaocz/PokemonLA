using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentButton : EnviromentSwitch
{
    

    /// <summary>
    /// ��ײ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Player" )
        {
            //δ���ı�ʱ ��δ�����²ſ��Ըı�
            if (ChangeState == changeState.idle && !isON)
            {
                StartCoroutine(SwitchON());
            }
        }

    }

}

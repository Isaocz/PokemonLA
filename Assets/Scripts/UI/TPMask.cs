using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPMask : MonoBehaviour
{
    public bool TPStart;
    float Timer;
    public float BlackTime; 
    bool TPEnd;
    public static TPMask In;
    Image Mask;

    bool isTPDoit;

    private void Awake()
    {
        
        In = this;
        Mask = gameObject.GetComponent<Image>();
    }



    // Update is called once per frame
    void Update()
    {
        if (TPStart)
        {
            GetComponent<Canvas>().sortingOrder = 21;
            Mask.color += new Color(0, 0, 0, Time.deltaTime);
            Timer += Time.deltaTime;
            if(BlackTime!=0 && Timer >= BlackTime)
            {
                TPEnd = true;
                TPStart = false;
                Mask.color = new Color(0, 0, 0, 1);
            }
        }
        if (TPEnd)
        {
            if (!isTPDoit)
            {
                PlayerControler player = GameObject.FindWithTag("Player").GetComponent<PlayerControler>();
                if (player.isTPMove)
                {
                    player.TPDoit();
                }
                isTPDoit = true;
            }
            GetComponent<Canvas>().sortingOrder = 19;
            Mask.color -= new Color(0, 0, 0, Time.deltaTime);
            Timer -= Time.deltaTime;
            if (Mask.color.a <= 0.01f)
            {
                TPEnd = false;
                TPStart = false;
                isTPDoit = false;
                Timer = 0;
                Mask.color = new Color(0, 0, 0, 0);
            }
        }
    }
}

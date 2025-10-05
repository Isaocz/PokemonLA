using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSoundBronzong : MonoBehaviour
{

    float Timer;
    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 1.2f) { Destroy(gameObject); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControler p = collision.GetComponent<PlayerControler>();
        if (p != null)
        {
            p.playerData.SpDBounsJustOneRoom--;
        }
    }
}

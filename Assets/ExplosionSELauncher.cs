using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSELauncher : MonoBehaviour
{
    public void SELauncher()
    {
        //±¨’®“Ù–ß
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonBasicSFXList.Explosion, transform.position);
        }
    }
}

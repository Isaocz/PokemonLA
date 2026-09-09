using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSE : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.CommonBasicSFXPlayer.Play(AudioManager.CommonUISFXList.ScorePanelFlower , Vector3.zero , true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

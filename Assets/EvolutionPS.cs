using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionPS : MonoBehaviour
{
    float EvolutionPSTimer;
    public Text text;

    private void Start()
    {
        BackGroundMusic.StaticBGM.BGM.volume = 0f;
    }

    private void Update()
    {
        EvolutionPSTimer += Time.deltaTime;
        if (EvolutionPSTimer >= 0.5)
        {
            text.transform.parent.parent.gameObject.SetActive(false);
        }
        if (EvolutionPSTimer >= 1.5)
        {
            BackGroundMusic.StaticBGM.BGM.volume += Time.deltaTime;
        }
        if (EvolutionPSTimer >= 3)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeText(string name1 , string name2)
    {
        text.text = "恭喜!" + name1 + "\n进化成" + name2 + "了!";
    }

    private void OnDestroy()
    {
        BackGroundMusic.StaticBGM.BGM.volume = 1f;
    }
}

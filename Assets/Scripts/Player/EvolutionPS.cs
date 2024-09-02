using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionPS : MonoBehaviour
{
    float EvolutionPSTimer;
    public Text text;
    public string Name;

    private void Start()
    {
        BackGroundMusic.StaticBGM.BGM.volume = 0f;
        text.text = " ......Ŷ����\n" + Name + "������......��";
    }

    private void Update()
    {
        EvolutionPSTimer += Time.deltaTime;
        if (EvolutionPSTimer >= 0.5)
        {
            text.transform.parent.parent.gameObject.SetActive(false);
            UISkillButton.Instance.isEscEnable = true;
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
        text.text = "��ϲ!" + name1 + "\n������" + name2 + "��!";
    }

    private void OnDestroy()
    {
        if (BackGroundMusic.StaticBGM != null) {
            BackGroundMusic.StaticBGM.BGM.volume = 1f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IsaoTalk : MonoBehaviour
{

    bool isLight;
    Image TalkPanel;
    Text TalkTitle1;
    Text TalkTitle2;
    Text TalkText1;
    Text TalkText2;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("FalseTalk", 7.5f);
        TalkPanel = GetComponent<Image>();
        TalkText1 = transform.GetChild(0).GetComponent<Text>();
        TalkText2 = transform.GetChild(1).GetComponent<Text>();
        TalkTitle1 = transform.GetChild(2).GetComponent<Text>();
        TalkTitle2 = transform.GetChild(3).GetComponent<Text>();

    }
    
    void FalseTalk()
    {
        isLight = true;
    }

    private void Update()
    {
        if (isLight)
        {
            TalkPanel.color -= new Color(0, 0, 0, Time.deltaTime);
            TalkText1.color -= new Color(0, 0, 0, 3f * Time.deltaTime);
            TalkText2.color -= new Color(0, 0, 0, 3f * Time.deltaTime);
            TalkTitle1.color -= new Color(0, 0, 0, 3f * Time.deltaTime);
            TalkTitle2.color -= new Color(0, 0, 0, 3f * Time.deltaTime);
            if (TalkPanel.color.a <= 0.05)
            {
                gameObject.SetActive(false);
                SceneManager.LoadSceneAsync(1);
            }
        }
    }
}

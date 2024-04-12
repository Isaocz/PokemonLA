using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillQuillityChange : MonoBehaviour
{
    Image i;
    public Sprite NormalSkillSprite;
    public Sprite PlusSkillSprite;
    public Sprite StudySkillSprite;



    private void Start()
    {
        i = transform.GetComponent<Image>();
    }


    public void ChangetoNormal()
    {
        if (i == null) { i = transform.GetComponent<Image>(); }
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Animator>().enabled = false;
        i.sprite = NormalSkillSprite;
    }
    public void ChangetoPlus()
    {
        if (i == null) { i = transform.GetComponent<Image>(); }
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<Animator>().enabled = true;
        i.sprite = PlusSkillSprite;
    }
    public void ChangetoStudy()
    {
        if (i == null) { i = transform.GetComponent<Image>(); }
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<Animator>().enabled = false;
        i.sprite = StudySkillSprite;
    }
    




}

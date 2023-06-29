using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : Skill
{
    public Sprite ReflectSpriteH;
    public Sprite ReflectSpriteV;
    SpriteRenderer RedlectSprite;
    public bool isThisSkillLightScreen;


    // Start is called before the first frame update
    void Start()
    {
        RedlectSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (transform.rotation.eulerAngles == new Vector3(0, 0, 0) || transform.rotation.eulerAngles == new Vector3(0, 0, 180)) { RedlectSprite.sprite = ReflectSpriteV; }
        else if (transform.rotation.eulerAngles == new Vector3(0, 0, 90) || transform.rotation.eulerAngles == new Vector3(0, 0, 270)) { RedlectSprite.sprite = ReflectSpriteH; }
        RedlectSprite.transform.rotation = Quaternion.Euler(Vector3.zero);
        if (transform.rotation.eulerAngles == new Vector3(0, 0, 0)) {  RedlectSprite.flipX = true; }
        if (transform.rotation.eulerAngles == new Vector3(0, 0, 270)) {
            RedlectSprite.transform.position += Vector3.down * 0.5f;
        }
        if (isThisSkillLightScreen) { player.isLightScreen = true; }
        else { player.isReflect = true; }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if(ExistenceTime < 0.6f)
        {
            RedlectSprite.color -= new Color(0, 0, 0, Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        if (isThisSkillLightScreen) { player.isLightScreen = false; }
        else { player.isReflect = false; }
    }

}

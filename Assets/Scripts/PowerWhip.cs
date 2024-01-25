using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerWhip : Skill
{

    SpriteRenderer Sprite;
    public GameObject PW;


    // Start is called before the first frame update
    void Start()
    {
        Sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        transform.GetChild(2).rotation = Quaternion.Euler(0,0,0);
        if (transform.rotation.eulerAngles.z == 180) { Sprite.flipY = true; }

        if (SkillFrom == 2)
        {
            for (int i = 0; i < player.InGressCount.Count ; i++)
            {
                NormalGress n = player.InGressCount[i].GetComponent<NormalGress>();
                GressPlayerINOUT g = player.InGressCount[i].GetComponent<GressPlayerINOUT>();
                if (player.InGressCount[i].gameObject.tag == "Grass")
                {
                    if (n != null) { Instantiate(PW, player.InGressCount[i].transform.position, transform.rotation).GetComponent<PowerWhip>().SkillFrom = 0; n.GrassDie(); }
                    if (g != null) { Instantiate(PW, player.InGressCount[i].transform.position, transform.rotation).GetComponent<PowerWhip>().SkillFrom = 0; g.GrassDie(); }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty")
        {
            Empty e = other.GetComponent<Empty>();
            if ( e != null)
            {
                HitAndKo(e);
            }
        }
    }

}

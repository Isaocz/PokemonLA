using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafStorm : Skill
{
    public SubLeafStorm SubLS;
    private bool isDown;
    // Start is called before the first frame update
    void Start()
    {
        isDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Empty"))
        {
            Empty target = collision.GetComponent<Empty>();
            if (target != null)
            {
                HitAndKo(target);
                if (!isDown && player.playerData.SpABounsJustOneRoom > -8)
                {
                    player.playerData.SpABounsJustOneRoom -= 2;
                    player.ReFreshAbllityPoint();
                    isDown = true;
                }
            }
        }
        if (SkillFrom == 2) {
            if (collision.CompareTag("Grass"))
            {
                NormalGress n = collision.GetComponent<NormalGress>();
                GressPlayerINOUT g = collision.GetComponent<GressPlayerINOUT>();
                if (collision.gameObject.tag == "Grass")
                {
                    if (n != null && !n.isDie) { Instantiate(SubLS, collision.transform.position, transform.rotation).GetComponent<SubLeafStorm>().player = player; n.GrassDie(); }
                    if (g != null && !g.isDie) { Instantiate(SubLS, collision.transform.position, transform.rotation).GetComponent<SubLeafStorm>().player = player; g.GrassDie(); }
                }
            }
        }
    }
}

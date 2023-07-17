using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : Skill
{
    // Start is called before the first frame update
    PlayerControler ParentPlayer;

    CircleCollider2D MistPlusCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        ParentPlayer = gameObject.transform.parent.GetComponent<PlayerControler>();
        if (ParentPlayer != null)
        {
            ParentPlayer.playerData.isMist = true;
        }
        if (SkillFrom == 2) { MistPlusCollider2D = GetComponent<CircleCollider2D>(); }
    }
    private void Update()
    {
        StartExistenceTimer();
        if (SkillFrom == 2) {
            if (ExistenceTime <= 3) {
                MistPlusCollider2D.radius -= Time.deltaTime * 1.5f;
            }
            if (ExistenceTime >= 14)
            {
                MistPlusCollider2D.radius += Time.deltaTime * 2f;
            }
        }
    }

    private void OnDestroy()
    {
        ParentPlayer.playerData.isMist = false;

    }


    List<Empty> MistPlusList = new List<Empty> {  };
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty") {
            Empty Target = other.GetComponent<Empty>();
            if (SkillFrom == 2) {
                if (!MistPlusList.Contains(Target))
                {
                    Target.Cold(15);
                }
                MistPlusList.Add(Target);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Covet : Skill
{
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
            SubEmptyBody Subtarget = collision.GetComponent<SubEmptyBody>();
            if (target != null)
            {
                if (target.DropItem != null)
                {
                    if (Subtarget == null) {
                        if ((target.EmptyBossLevel == Empty.emptyBossLevel.Boss || target.EmptyBossLevel == Empty.emptyBossLevel.EndBoss))
                        {
                            Instantiate(target.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                            Instantiate(target.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                            Instantiate(target.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                        }
                        else
                        {
                            if (target.IsHaveDropItem)
                            {
                                target.EmptyDrop();
                            }
                        }
                    }
                    
                    target.DropItem = null;
                }
                if (Subtarget != null && Subtarget.ParentEmpty.DropItem != null)
                {
                    if ((Subtarget.ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.Boss || Subtarget.ParentEmpty.EmptyBossLevel == Empty.emptyBossLevel.EndBoss))
                    {
                        Instantiate(Subtarget.ParentEmpty.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                        Instantiate(Subtarget.ParentEmpty.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                        Instantiate(Subtarget.ParentEmpty.DropItem, transform.position, Quaternion.identity, transform.parent).GetComponent<RandomSkillItem>().isLunch = true;
                        Subtarget.ParentEmpty.DropItem = null;
                    }
                    else
                    {
                        if (Subtarget.ParentEmpty.IsHaveDropItem)
                        {
                            Subtarget.ParentEmpty.EmptyDrop();
                            Subtarget.ParentEmpty.DropItem = null;
                        }
                    }
                    Subtarget.ParentEmpty.DropItem = null;
                }
                HitAndKo(target);
            }
        }
    }

}

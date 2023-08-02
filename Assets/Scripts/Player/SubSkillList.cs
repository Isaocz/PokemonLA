using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSkillList : MonoBehaviour
{
    // Start is called before the first frame update

    public List<SubSkill> SubSList = new List<SubSkill> { };
    PlayerControler player;

    public void CopyList(SubSkillList OtherList)
    {
        SubSList = OtherList.SubSList;
    }

    private void Start()
    {
        player = GetComponent<PlayerControler>();
    }

    public void CallSubSkill(Skill MainSkill )
    {

        foreach (var Sub in SubSList)
        {
            SubSkill SubSkillObj = null;
            Vector3 Direction = player.look;
            if (!Sub.isNotDirection)
            {
                if (player.look.Equals(new Vector2(1, 0)))
                {
                    SubSkillObj = Instantiate(Sub, transform.position + (Vector3.up * 0.4f) + (Direction * Sub.DirctionDistance), Quaternion.Euler(0, 0, 0), Sub.isNotMoveWithPlayer ? null : transform);
                }
                else if (player.look.Equals(new Vector2(-1, 0)))
                {
                    SubSkillObj = Instantiate(Sub, transform.position + (Vector3.up * 0.4f) + (Direction * Sub.DirctionDistance), Quaternion.Euler(0, 0, 180), Sub.isNotMoveWithPlayer ? null : transform);
                }
                else if (player.look.Equals(new Vector2(0, 1)))
                {
                    SubSkillObj = Instantiate(Sub, transform.position + (Vector3.up * 0.4f) + (Direction * Sub.DirctionDistance), Quaternion.Euler(0, 0, 90), Sub.isNotMoveWithPlayer ? null : transform);
                }
                else if (player.look.Equals(new Vector2(0, -1)))
                {
                    SubSkillObj = Instantiate(Sub, transform.position + (Vector3.up * 0.4f) + (Direction * Sub.DirctionDistance), Quaternion.Euler(0, 0, 270), Sub.isNotMoveWithPlayer ? null : transform);
                }
            }
            else
            {
                SubSkillObj = Instantiate(Sub, transform.position, Quaternion.identity, Sub.isNotMoveWithPlayer ? null : transform);
            }
            SubSkillObj.MainSkill = MainSkill;
            SubSkillObj.subskill = Sub;
            SubSkillObj.player = player;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBaby : Baby
{
    // Start is called before the first frame update

    protected GameObject Target;
    Animator animator;
    Vector3 LastPosition;
    float Speed;
    public bool isInANewRoom;
    bool isInANewRoom2;
    protected PlayerControler TargetPlayer;



    public void FollowBabyStart()
    {
        animator = GetComponent<Animator>();
        LastPosition = transform.position;
        Target = transform.parent.parent.parent.gameObject;
        Speed = 4;
        if (Target.GetComponent<PlayerControler>() != null) { TargetPlayer = Target.GetComponent<PlayerControler>(); }
        if(GetChildNum() != 0)
        {
            Target = transform.parent.GetChild(GetChildNum()-1).gameObject;
        }
    }

    int GetChildNum()
    {
        int i = 0;
        while (transform.parent.GetChild(i) != transform)
        {
            i++;
        }
        return i;
    }

    // Update is called once per frame
    public void FollowBabyUpdate()
    {
        if(isInANewRoom2 == false && TargetPlayer.InANewRoom == true) { isInANewRoom = true; isInANewRoom2 = true; }
        if (isInANewRoom2 == true && TargetPlayer.InANewRoom == false) { isInANewRoom2 = false; }
        if (isInANewRoom == true) {  transform.position = TargetPlayer.transform.position; isInANewRoom = false; }

        //Debug.Log(TargetPlayer.InANewRoom);
        Vector2 direction = (Target.transform.position - transform.position).normalized;
        if ((Target.transform.position - transform.position).magnitude < 3)
        {
            Speed = Mathf.Clamp( 4 - (4*(3-(Target.transform.position - transform.position).magnitude))/1.5f , 0 ,4);
        }
        else
        {
            Speed = 4;
        }

        if ((Target.transform.position - transform.position).magnitude > 1.7f) {
            
            //Debug.Log((Target.transform.position - transform.position).magnitude);
            Vector3 position = transform.position;
            position.x += Time.deltaTime * direction.x * Speed;
            position.y += Time.deltaTime * direction.y * Speed;
            transform.position = position;
        }
        if (LastPosition.x - transform.position.x > 0) { animator.SetFloat("LookX", 1); }
        else if (LastPosition.x - transform.position.x < 0) { animator.SetFloat("LookX", -1); }
        if (LastPosition.y - transform.position.y > 0) { animator.SetFloat("LookY", 1); }
        else if (LastPosition.y - transform.position.y < 0) { animator.SetFloat("LookY", -1); }
        animator.SetFloat("Speed", (LastPosition - transform.position).magnitude);
        LastPosition = transform.position;

    }
}

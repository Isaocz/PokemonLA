using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroBall : Skill
{
    Rigidbody2D PlayerRigibody;
    Vector2 Direction;
    bool MoveStop;

    Vector2 PlayerLookAt;
    Vector2 d;

    BoxCollider2D GryoBallBoxCollider;

    Empty Target;
    List<Vector3> TagetPositionList = new List<Vector3> { };
    List<float> SpeedList = new List<float> { };

    // Start is called before the first frame update
    void Start()
    {

        PlayerRigibody = player.GetComponent<Rigidbody2D>();
        Direction = transform.rotation * Vector2.right;
        animator = transform.GetComponent<Animator>();
        player.isCanNotMove = true;
        PlayerLookAt = player.look;

        animator.SetFloat("LookX" , PlayerLookAt.x);
        animator.SetFloat("LookY" , PlayerLookAt.y);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        GryoBallBoxCollider = transform.GetComponent<BoxCollider2D>();
        BoxCollider2D PlayerBoxCollider = player.transform.GetComponent<BoxCollider2D>();
        var s = GryoBallBoxCollider.size;
        s.x = PlayerBoxCollider.size.x + 0.15f;
        s.y = PlayerBoxCollider.size.y + 0.15f;
        GryoBallBoxCollider.offset = PlayerBoxCollider.offset;
        GryoBallBoxCollider.edgeRadius = PlayerBoxCollider.edgeRadius;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();

        if (ExistenceTime > 1.8f)
        {
            player.transform.GetChild(3).localScale -= new Vector3(5.0f * Time.deltaTime, 5.0f * Time.deltaTime, 0);
        }

        if (ExistenceTime <= 0.5f && MoveStop == false) { MoveStopF(); }
        if (!MoveStop)
        {
            Vector2 postion = PlayerRigibody.position;
            postion.x += Direction.x * 2.5f * player.speed * Time.deltaTime;
            postion.y += Direction.y * 2.5f * player.speed * Time.deltaTime;
            PlayerRigibody.position = postion;
        }
    }

    private void FixedUpdate()
    {
        if (Target != null)
        {

            TagetPositionList.Add(new Vector3(Target.transform.position.x, Target.transform.position.y, ((TagetPositionList.Count == 0) ? 0 : Time.deltaTime + TagetPositionList[TagetPositionList.Count - 1].z)));
            if (TagetPositionList.Count >= 2)
            {
                SpeedList.Add((new Vector2(TagetPositionList[TagetPositionList.Count - 1].x, TagetPositionList[TagetPositionList.Count - 1].y) - new Vector2(TagetPositionList[TagetPositionList.Count - 2].x, TagetPositionList[TagetPositionList.Count - 2].y)).magnitude / (TagetPositionList[TagetPositionList.Count - 1].z - TagetPositionList[TagetPositionList.Count - 2].z));
            }
            if (TagetPositionList.Count >= 6)
            {
                Damage = (int)(Power((float)MoveSpeed()) * Power2(player.speed) * ((SkillFrom == 2)? ( (((float)player.DefAbilityPoint) / ((float)Target.DefAbilityPoint)) * (((float)player.SpdAbilityPoint) / ((float)Target.SpdAbilityPoint)) ) : 1));
                Debug.Log(((SkillFrom == 2) ? ((((float)player.DefAbilityPoint) / ((float)Target.DefAbilityPoint)) * (((float)player.SpdAbilityPoint) / ((float)Target.SpdAbilityPoint))) : 1));
                Debug.Log(Damage);
                HitAndKo(Target);
                Target = null;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Empty" || other.gameObject.tag == "Room" || other.gameObject.tag == "Enviroment" || other.gameObject.tag == "Water")
        {
            MoveStopF();
            if (other.gameObject.tag == "Empty")
            {
                Empty target = other.gameObject.GetComponent<Empty>();
                if (target != null)
                {
                    Target = target;
                }
            }
        }
    }



    public void MoveStopF()
    {
        player.transform.GetChild(3).localScale = player.PlayerLocalScal;
        player.isCanNotMove = false;
        transform.GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Over");
        MoveStop = true;
        ExistenceTime = 0.5f;
    }


    float MoveSpeed()
    {
        float OutPut = 0;
        int Count = 0;
        for (int i = 0; i < SpeedList.Count; i++)
        {
            for (int j = i; j < SpeedList.Count; j++)
            {
                if (SpeedList[i] - SpeedList[j] < 1) { Count++; }
            }
            if (Count > SpeedList.Count / 1.5f) { OutPut = SpeedList[i]; break; }
        }

        return OutPut;
    }

    int Power(float s)
    {
        int OutPut = 20;
        if (s > 0 && s < 2.5f)
        {
            OutPut = 20;
        }
        else if (s > 2.5f && s < 4.5f)
        {
            OutPut = 40;
        }
        else if (s > 4.5f && s < 7f)
        {
            OutPut = 60;
        }
        else if (s > 7f && s < 10f)
        {
            OutPut = 80;
        }
        else if (s > 10f && s < 15f)
        {
            OutPut = 100;
        }
        else if (s > 15f)
        {
            OutPut = 120;
        }
        return OutPut;
    }

    float Power2(float s)
    {
        float OutPut = 1.0f;
        if (s > 6.0 && s <= 7.0f)
        {
            OutPut = 1.0f;
        }
        else if (s > 7.0f && s <= 8.5f)
        {
            OutPut = 0.8f;
        }
        else if (s > 8.5f)
        {
            OutPut = 0.6f;
        }
        else if (s > 4.8f && s <= 6.0f)
        {
            OutPut = 1.2f;
        }
        else if (s > 3.6f && s <= 4.8f)
        {
            OutPut = 1.44f;
        }
        else if (s <= 3.6f)
        {
            OutPut = 1.6f;
        }
        return OutPut;
    }
}

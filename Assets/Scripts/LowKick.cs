using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowKick : Skill
{
    Empty Target;
    List<Vector3> TagetPositionList = new List<Vector3> { };
    List<float> SpeedList = new List<float> { };
    float Timer;



    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        transform.GetChild(1).rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
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
                Damage = Power(MoveSpeed());
                Debug.Log(MoveSpeed() + "+" + Damage);
                HitAndKo(Target);
                Target = null;
                Destroy(gameObject);
            }
        }
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
            if (SkillFrom == 2) { CTLevel += 1; }
        }
        else if (s > 4.5f && s < 7f)
        {
            OutPut = 60;
            if (SkillFrom == 2) { CTLevel += 1;CTDamage += 1; }
        }
        else if (s > 7f && s < 10f)
        {
            OutPut = 80;
            if (SkillFrom == 2) { CTLevel += 2; CTDamage += 1; }
        }
        else if (s > 10f && s < 15f)
        {
            OutPut = 100;
            if (SkillFrom == 2) { CTLevel += 2; CTDamage += 2; }
        }
        else if (s > 15f)
        {
            OutPut = 120;
            if (SkillFrom == 2) { CTLevel += 2; CTDamage += 2; }
        }
        return OutPut;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Empty" && Target == null)
        {
            Empty e = other.GetComponent<Empty>();
            if (e != null) { Target = e; }
        }
    }
}

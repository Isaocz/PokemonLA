using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassKnot : GrassSkill
{

    public GameObject GrassPS;
    bool isFreeEndPS;
    Empty Target;
    List<Vector3> TagetPositionList = new List<Vector3> { };
    List<float> SpeedList = new List<float> { };
    public GameObject GK;
    float Timer;



    // Start is called before the first frame update
    void Start()
    {
        Instantiate(GrassPS, transform.position, Quaternion.identity);
        if (SkillFrom == 2)
        {
            for (int i = 0; i < player.InGressCount.Count; i++)
            {
                NormalGress n = player.InGressCount[i].GetComponent<NormalGress>();
                GressPlayerINOUT g = player.InGressCount[i].GetComponent<GressPlayerINOUT>();
                if (player.InGressCount[i].gameObject.tag == "Grass")
                {
                    if (n != null) { Instantiate(GK, player.InGressCount[i].transform.position, transform.rotation).GetComponent<GrassKnot>().SkillFrom = 0; n.GrassDie(); }
                    if (g != null) { Instantiate(GK, player.InGressCount[i].transform.position, transform.rotation).GetComponent<GrassKnot>().SkillFrom = 0; g.GrassDie(); }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        if (ExistenceTime < 0.1f && !isFreeEndPS)
        {
            OnKnotDestroy();
        }



    }

    private void FixedUpdate()
    {
        if (Target != null)
        {

            TagetPositionList.Add(new Vector3(Target.transform.position.x, Target.transform.position.y,  ((TagetPositionList.Count == 0)? 0 : Time.deltaTime+ TagetPositionList[TagetPositionList.Count-1].z)   )   );
            if (TagetPositionList.Count >= 2)
            {
                SpeedList.Add((new Vector2(TagetPositionList[TagetPositionList.Count - 1].x, TagetPositionList[TagetPositionList.Count - 1].y) - new Vector2(TagetPositionList[TagetPositionList.Count - 2].x, TagetPositionList[TagetPositionList.Count - 2].y)).magnitude / (TagetPositionList[TagetPositionList.Count - 1].z - TagetPositionList[TagetPositionList.Count - 2].z));
            }
            if (TagetPositionList.Count >= 6)
            {
                Damage = Power(MoveSpeed());
                Debug.Log(Damage);
                HitAndKo(Target);
                Target = null;
                OnKnotDestroy();
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

    int Power( float s )
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



    private void OnKnotDestroy()
    {
        isFreeEndPS = true;
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(3).parent = transform.parent;
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

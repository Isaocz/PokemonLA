using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wish : Skill
{
    [SerializeField]
    private float rotationSpeed;
    private float timer;
    float movetime = 0; //Ã· æ∆Ì‘∏ ß∞‹ ±º‰
    bool isMove;
    Vector3 StartPosition;

    public GameObject starX;

    void Start()
    {
        Invoke("Heal", 5f);
        Invoke("DestroyStar", 2.5f);
        StartPosition = player.transform.position;
        transform.GetChild(5).parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        StartExistenceTimer();
        if (timer < 2f)
        {//ŒÂ√Î∫ÛÕ£÷π–˝◊™
            rotationSpeed = 80f + Mathf.Pow(Mathf.Exp(1.5f) * timer, 2);
            transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
            JudgeisMove();
        }

        //∆Ì‘∏ ß∞‹
        if (isMove && movetime < 1f && starX)
        {
            movetime += Time.deltaTime;
            starX.SetActive(true);
            SpriteRenderer sr = starX.GetComponent<SpriteRenderer>();
            if(movetime < 0.2f)
            {
                float t = movetime / 0.2f;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, t);
            }
            else if (movetime > 0.8f)
            {
                float t = (1 - movetime) / 0.2f;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, t);
            }
            else if(movetime > 1f)
            {
                 starX.SetActive(false);
            }
        }
    }

    //≈–∂œÕÊº“ «∑Ò¥Û∑˘“∆∂Ø
    void JudgeisMove()
    {
        if ((StartPosition - player.transform.position).magnitude >= ((SkillFrom==2 )?1.5f: 1.0f)) { isMove = true; }
    }

    //œ˙ªŸ–«–«
    void DestroyStar()
    {
        for (int i = 0; i < 4; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void Heal()
    {

        if (!isMove) {

            if (SkillFrom != 2)
            {
                Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp / 4, PokemonType.TypeEnum.IgnoreType);
            }
            else
            {
                Pokemon.PokemonHpChange(null, player.gameObject, 0, 0, player.maxHp * 2 / 5, PokemonType.TypeEnum.IgnoreType);
            }
            transform.Rotate(0, 0, 180f);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

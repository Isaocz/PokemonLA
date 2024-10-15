using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mewEnterEffect : MonoBehaviour
{
    public float RotateSpeed;
    public float InitialMoveSpeed;
    public float DeleteTime;

    private GameObject Star;
    private GameObject[] Stars = new GameObject[PokemonType.TypeColor.Length];
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Star = transform.GetChild(0).gameObject;
        Stars[0] = Star;
        timer = 0f;
        for (int i = 0; i < Stars.Length - 1; i++)
        {
            GameObject StarN = Instantiate(Star, this.gameObject.transform);//复制星星
            Stars[i+1] = StarN;
        }
        for(int i = 0; i < Stars.Length; i++)
        {
            float radians = (i * 360f / Stars.Length) / 180f * Mathf.PI;
            Stars[i].transform.position = transform.position + new Vector3(3 * Mathf.Cos(radians), 3 * Mathf.Sin(radians));//初始半径和均分
            Stars[i].transform.rotation = Quaternion.Euler(0f, 0f, radians * 180f / Mathf.PI);
            Stars[i].GetComponent<SpriteRenderer>().color = PokemonType.TypeColor[i];
            Stars[i].GetComponent<TrailRenderer>().startColor = PokemonType.TypeColor[i];
            Stars[i].GetComponent<TrailRenderer>().endColor = PokemonType.TypeColor[(i + 1) > (PokemonType.TypeColor.Length - 1) ? 0 : i + 1];//如果大于阈值则把meta设为0
        }
        Destroy(this.gameObject, DeleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].transform.RotateAround(Stars[i].transform.parent.position, Vector3.forward, RotateSpeed);
            float moveSpeed;
            if (timer < 1f)//向内移动
            {
                moveSpeed = Mathf.Lerp(InitialMoveSpeed, 0f, timer / 1f);
                Stars[i].transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            }
            else if(timer > 3f)//向外移动
            {
                moveSpeed = Mathf.Exp(timer - 3f) - 1f;
                Stars[i].transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            }

            if (timer < 0.5f) //渐入
            {
                float t = timer / 0.5f;
                float colorLerp = Mathf.Lerp(0f, PokemonType.TypeColor[i].z, t);
                Stars[i].GetComponent<SpriteRenderer>().color = new Color(PokemonType.TypeColor[i].w, PokemonType.TypeColor[i].x, PokemonType.TypeColor[i].y, colorLerp);
            }
        }
    }
}

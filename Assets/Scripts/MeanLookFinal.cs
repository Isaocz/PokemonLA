using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookFinal : MonoBehaviour
{
    public List<Transform> flamesList;
    public float rotationSpeed; // 旋转速度

    private PlayerControler player;
    private SpriteRenderer rend;
    public float fadeDuration = 1f;
    public float radius = 9f;

    private bool fading = false;
    private float timer = 0f;
    private Color startColor;
    private Color targetColor;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        rend = GetComponent<SpriteRenderer>();
        // 对象的初始颜色
        startColor = new Color(0f, 0f, 0f, 0f);
        // 设置目标颜色的透明度为1，其他颜色保持不变
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAngle = rotationSpeed * Time.deltaTime;
        foreach (Transform flame in flamesList)
        {
            flame.RotateAround(transform.position, transform.forward, rotationAngle);
        }
        // 计算玩家和对象之间的距离
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // 判断是否在半径范围内
        if (distance >= radius)
        {
            if (!fading)
            {
                StartFadeIn();
            }
        }
        else
        {
            if (!fading)
            {
                StartFadeOut();
            }
        }

        // 渐变效果
        if (fading)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            rend.material.color = Color.Lerp(startColor, targetColor, t);

            if (timer >= fadeDuration)
            {
                fading = false;
            }
        }
    }
    private void StartFadeIn()
    {
        fading = true;
        timer = 0f;
        startColor = rend.material.color;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
    }

    private void StartFadeOut()
    {
        fading = true;
        timer = 0f;
        startColor = rend.material.color;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}

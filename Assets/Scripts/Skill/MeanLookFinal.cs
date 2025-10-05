using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanLookFinal : MonoBehaviour
{
    public List<Transform> flamesList;
    public float rotationSpeed; // ��ת�ٶ�

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
        // ����ĳ�ʼ��ɫ
        startColor = new Color(0f, 0f, 0f, 0f);
        // ����Ŀ����ɫ��͸����Ϊ1��������ɫ���ֲ���
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
        // ������ҺͶ���֮��ľ���
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // �ж��Ƿ��ڰ뾶��Χ��
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

        // ����Ч��
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

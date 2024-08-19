using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MewDashReticle : MonoBehaviour
{
    public float radius;
    public float angleOffset;
    public int skillTimes;
    public Vector3 offset = new Vector3(0f, 0.5f, 0f);
    private PlayerControler player;
    private Mew mew;
    private SpriteRenderer sr;
    private float timer;

    private Vector3 dashStartPosition;
    private Vector3 dashEndPosition;

    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        mew = FindObjectOfType<Mew>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(player != null && mew != null) 
        {
            timer += Time.deltaTime;
            if (timer > 1.3f)
            {
                timer = 0;
                sr.enabled = true;
                if(skillTimes == 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    skillTimes--;
                }
            }
            else if (timer > 0.8f && timer <= 1.3f)
            {
                sr.enabled = false;

                float t = Mathf.Sin((timer - 0.8f) * Mathf.PI * 2f / 2f);
                //��ʼ��Ϊ ( ��ʼ�ƶ�ʱ�� , 0 )
                //�ɹ�ʽ T = 2pi / w ���Եõ� w ��ֵ
                mew.transform.position = Vector3.Lerp(dashStartPosition, dashEndPosition, t);

            }
            else if (timer <= 0.6f)
            {
                //���Ŷ���ĸı�
                Vector3 direction = (mew.transform.position - player.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                transform.position = mew.transform.position + offset - (Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius);

                //�λõ��߼�
                dashStartPosition = mew.transform.position + offset;
                dashEndPosition = player.transform.position - 1.5f * direction + offset;
            }
        }

    }
}

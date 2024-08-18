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
            if (timer > 3f)
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
            else if (timer > 1.3f && timer <= 3f)
            {
                sr.enabled = false;

                float t = Mathf.Sin((timer - 1.3f) * 2 * Mathf.PI / 6.8f);
                mew.transform.position = Vector3.Lerp(dashStartPosition, dashEndPosition, t);

            }
            else if (timer <= 1f)
            {
                //附着对象的改变
                Vector3 direction = (mew.transform.position - player.transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle + angleOffset);
                transform.position = mew.transform.position + (Quaternion.Euler(0f, 0f, angle) * Vector2.right * radius + new Vector3(0f, 0.5f, 0f));

                //梦幻的逻辑
                dashStartPosition = mew.transform.position;
                dashEndPosition = player.transform.position + 1.5f * direction;
            }
        }

    }
}

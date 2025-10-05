using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherDance : Skill
{
    float DanceTimer;
    int DanceCount;
    List<Empty> FeatherDanceEmptyList = new List<Empty> { };
    List<Empty> FeatherDancePlusEmptyList = new List<Empty> { };

    Vector3 StartP;
    Vector3 StartS;

    // Start is called before the first frame update
    void Start()
    {
        player.isCanNotMove = true;
        player.animator.SetFloat("Speed", 0);
        player.look = new Vector2(0, -1);
        player.animator.SetFloat("LookX", 0);
        player.animator.SetFloat("LookY", -1);
        StartP = player.transform.GetChild(3).localPosition;
        StartS = player.transform.GetChild(3).localScale;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
        DanceTimer += Time.deltaTime;

        if (DanceTimer >= 0.2)
        {
            if (DanceTimer >= 0.2f && DanceTimer < 0.3f)
            {
                player.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(player.transform.GetChild(3).localScale.x + Time.deltaTime, 1.0f, 1.1f), Mathf.Clamp(player.transform.GetChild(3).localScale.y - Time.deltaTime, 0.9f, 1.0f), player.transform.GetChild(3).localScale.z);
            }
            else if (DanceTimer >= 0.3f && DanceTimer < 0.4f)
            {
                player.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(player.transform.GetChild(3).localScale.x - Time.deltaTime, 1.0f, 1.1f), Mathf.Clamp(player.transform.GetChild(3).localScale.y + Time.deltaTime, 0.9f, 1.0f), player.transform.GetChild(3).localScale.z);
            }
            else if (DanceTimer >= 0.4f && DanceTimer < 0.6f)
            {
                player.transform.GetChild(3).position = new Vector3(player.transform.GetChild(3).position.x, player.transform.GetChild(3).position.y + 7 * Time.deltaTime, player.transform.GetChild(3).position.z);
            }
            else if (DanceTimer >= 0.6f && DanceTimer < 0.8f)
            {
                player.transform.GetChild(3).position = new Vector3(player.transform.GetChild(3).position.x, player.transform.GetChild(3).position.y - 7 * Time.deltaTime, player.transform.GetChild(3).position.z);
            }
            else if (DanceTimer >= 0.8f && DanceTimer < 0.9f)
            {
                player.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(player.transform.GetChild(3).localScale.x + Time.deltaTime, 1.0f, 1.1f), Mathf.Clamp(player.transform.GetChild(3).localScale.y - Time.deltaTime, 0.9f, 1.0f), player.transform.GetChild(3).localScale.z);
            }
            else if (DanceTimer >= 0.9f && DanceTimer < 1.0f)
            {
                player.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(player.transform.GetChild(3).localScale.x - 2 * Time.deltaTime, 1.0f, 1.1f), Mathf.Clamp(player.transform.GetChild(3).localScale.y + 2 * Time.deltaTime, 0.9f, 1.0f), player.transform.GetChild(3).localScale.z);
            }
            else if (DanceTimer >= 1.0f && DanceTimer < 1.1f)
            {
                player.transform.GetChild(3).localScale = new Vector3(Mathf.Clamp(player.transform.GetChild(3).localScale.x + Time.deltaTime, 1.0f, 1.1f), Mathf.Clamp(player.transform.GetChild(3).localScale.y - Time.deltaTime, 0.9f, 1.0f), player.transform.GetChild(3).localScale.z);
            }
            if (DanceTimer >= 1.2) {
                DanceTimer -= 1.0f; DanceCount++;
                if (DanceCount >= 3)
                {
                    transform.DetachChildren();
                    Destroy(gameObject);
                }
            }
        }

    }

    private void OnDestroy()
    {

        player.isCanNotMove = false;
        player.transform.GetChild(3).localPosition = player.PlayerLocalPosition;
        player.transform.GetChild(3).localScale = player.PlayerLocalScal;
    }

    public void FeatherDanceEffect(Empty target)
    {
        if (!FeatherDanceEmptyList.Contains(target))
        {
            FeatherDanceEmptyList.Add(target);
            target.AtkChange(-2,10*target.OtherStateResistance);
        }
    }

    public void FeatherDancePlusEffect(Empty target)
    {
        if (!FeatherDancePlusEmptyList.Contains(target))
        {
            FeatherDancePlusEmptyList.Add(target);
            target.DefChange(-2, 10 * target.OtherStateResistance);
            target.SpAChange(-2, 10 * target.OtherStateResistance);
            target.SpDChange(-2, 10 * target.OtherStateResistance);
        }
    }

}

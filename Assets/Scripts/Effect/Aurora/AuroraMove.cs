using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuroraMove : MonoBehaviour
{
    public Vector2 SpeedRange;
    float Speed;

    float Timer;

    float StartY;

    public float SpeedAlpha;

    Vector3 MoveDir = Vector3.down;

    // Start is called before the first frame update
    void Start()
    {
        DirChange();
        Speed = Random.Range(SpeedRange.x, SpeedRange.y);
        StartY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + MoveDir * Speed * SpeedAlpha;
        if (Mathf.Abs( transform.position.y - StartY ) > 1.0f) { MoveDir = -MoveDir; }
    }

    IEnumerator DirChangeByTime()
    {
        yield return new WaitForSeconds(1.0f);
        DirChange();
    }


    void DirChange()
    {
        float f = Random.Range(0.0f, 1.0f);
        if (f < 0.33f)
        {
            MoveDir = Vector3.down;
        }
        else if (f >= 0.33f && f < 0.67f)
        {
            MoveDir = Vector3.up;
        }
        else
        {
            MoveDir = Vector3.zero;
        }
    }


}

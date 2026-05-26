using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawTest : MonoBehaviour
{


    public float LSpeed;
    public float RSpeed;

    public float cTime = 1.0f;

    Vector2 Dir = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cTime >= 0.0f) {
            cTime -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x + Dir.x * Time.deltaTime * LSpeed, transform.position.y + Dir.y * Time.deltaTime * LSpeed, 0);
            transform.rotation = Quaternion.Euler(0, 0, _mTool.Angle_360Y(Dir, Vector2.right));
            Dir = Quaternion.AngleAxis(RSpeed * Time.deltaTime, Vector3.forward) * Dir;
        }
        
    }
}

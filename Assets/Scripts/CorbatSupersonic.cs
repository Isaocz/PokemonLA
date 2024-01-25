using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorbatSupersonic : MonoBehaviour
{

    public CorbatSupersonic ChildSupersonic;
    public int ChildCount;
    bool isChildBorn;

    float Timer;
    CircleCollider2D TopCollider2D;
    bool isMoveOver;
    Vector2 MoveD;

    private void Start()
    {
        TopCollider2D = GetComponent<CircleCollider2D>();
        MoveD = (Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward) * Vector2.right).normalized;
        var COffest = TopCollider2D.offset;
        COffest.x = 0;Timer = 0; isMoveOver = false; isChildBorn = false;
        TopCollider2D.offset = COffest;
    }

    private void Update()
    {
        if (Timer < 1.0f)
        {
            Timer += Time.deltaTime;
            var COffest = TopCollider2D.offset;
            COffest.x = Mathf.Clamp(COffest.x+ 13 * Time.deltaTime , 0 ,12);
            TopCollider2D.offset = COffest;
        }
        if (!isMoveOver)
        {
            transform.position += new Vector3( MoveD.x * 5.0f * Time.deltaTime , MoveD.y * 5.0f * Time.deltaTime , 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isChildBorn && Timer > 0.05f && ChildCount <= 4)
        {
            if (other.tag == "Room" || other.tag == "Enviroment")
            {
                Vector3 TouchPoint = transform.position + new Vector3(MoveD.x * TopCollider2D.offset.x, MoveD.y * TopCollider2D.offset.x);
                Debug.Log(TouchPoint + name);
                isChildBorn = true;

                if (Mathf.Abs(TouchPoint.x) >= 11.0f && Mathf.Abs(TouchPoint.y) <= 6.2f)
                {
                    Quaternion x = Quaternion.Euler(new Vector3(0, 0, 180 - transform.rotation.eulerAngles.z));
                    Instantiate(ChildSupersonic, TouchPoint, x).ChildCount = ChildCount + 1;
                }
                else if (Mathf.Abs(TouchPoint.x) <= 11.0f && Mathf.Abs(TouchPoint.y) >= 6.2f)
                {
                    Quaternion x = Quaternion.Euler(new Vector3(0, 0, 0 - transform.rotation.eulerAngles.z));
                    Instantiate(ChildSupersonic, TouchPoint, x).ChildCount = ChildCount + 1;
                }
                else if (Mathf.Abs(TouchPoint.x) >= 11.0f && Mathf.Abs(TouchPoint.y) >= 6.2f) {
                    Quaternion x = Quaternion.Euler(new Vector3(0, 0, 180 + transform.rotation.eulerAngles.z));
                    Instantiate(ChildSupersonic, TouchPoint, x).ChildCount = ChildCount + 1;
                }
            }
        }
    }

    // Start is called before the first frame update
    private void OnParticleCollision(GameObject other)
    {

        PlayerControler p = other.GetComponent<PlayerControler>();
        if (p != null)
        {
            p.ConfusionFloatPlus(0.2f);
        }
    }
}

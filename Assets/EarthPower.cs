using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPower : Skill
{

    Vector2 direction;
    Vector3 StartPostion;
    bool isBlest;
    GameObject Move;
    GameObject Trail;
    GameObject Hit;

    // Start is called before the first frame update
    void Start()
    {
        StartPostion = transform.position;
        Trail = transform.GetChild(0).gameObject;
        Move = transform.GetChild(1).gameObject;
        Hit = transform.GetChild(2).gameObject;

        direction = (transform.rotation * Vector2.right).normalized;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void FixedUpdate()
    {
        if (!isBlest)
        {
            Vector3 postion = transform.position;
            postion.x += direction.x * 4.5f * Time.deltaTime;
            postion.y += direction.y * 4.5f * Time.deltaTime;
            transform.position = postion;
            if ((StartPostion - transform.position).magnitude > MaxRange)
            {
                isBlest = true;
                Move.SetActive(false);Hit.SetActive(true);
            }
        }
    }

    bool isUp;
    float X = 0.2f;

    // Update is called once per frame
    void Update()
    {
        if (!isBlest)
        {

            if(Mathf.Abs(direction.x-0) <= 0.01)
            {
                if (isUp) { Trail.transform.position -= new Vector3(Time.deltaTime * 10, 0, 0);}
                else { Trail.transform.position += new Vector3(Time.deltaTime * 10, 0, 0); }
                if(Trail.transform.localPosition.x >= X) { isUp = true; X = Random.Range(0.2f , 0.35f); }
                else if (Trail.transform.localPosition.x <= -X){ isUp = false; X = Random.Range(0.2f, 0.35f); }
            }
            else if (Mathf.Abs(direction.y - 0) <= 0.01)
            {
                if (isUp) { Trail.transform.position -= new Vector3(0, Time.deltaTime * 10, 0); }
                else { Trail.transform.position += new Vector3(0, Time.deltaTime * 10, 0); }
                if (Trail.transform.localPosition.y >= X) { isUp = true; X = Random.Range(0.2f, 0.35f); }
                else if (Trail.transform.localPosition.y <= -X) { isUp = false; X = Random.Range(0.2f, 0.35f); }
            }
        }
        else
        {
            StartExistenceTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Room" || Other.tag == "Enviroment" || Other.tag == "Empty")
        {
            isBlest = true;
            Trail.transform.GetChild(0).GetComponent<TrailRenderer>().time = 0.5f;
            Move.SetActive(false); Hit.SetActive(true);
        }
    }

}

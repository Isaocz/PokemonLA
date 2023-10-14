using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBornGress : MonoBehaviour
{
    public GameObject PS;
    public GameObject Gress;
    public bool isNotEmpty;


    private void Start()
    {
        Instantiate(PS, transform.position, Quaternion.identity);
    }

    public void BornGress()
    {
        Instantiate(Gress , transform.position , Quaternion.identity );
        Destroy(gameObject);
    }



}

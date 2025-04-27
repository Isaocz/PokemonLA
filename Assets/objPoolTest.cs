using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objPoolTest : MonoBehaviour
{
    public int preheatNum;
    public int repeatNumPerFrame;
    public GameObject obj;
    private PlayerControler player;
    private bool objpoolOnTest;
    private bool objpoolOffTest;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        ObjectPoolManager.PreheatPool(obj, preheatNum);
        objpoolOnTest = false;
        objpoolOffTest = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            objpoolOnTest = !objpoolOnTest;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            objpoolOffTest = !objpoolOffTest;
        }
        
        if (objpoolOnTest)
        {
            for(int i = 0; i<repeatNumPerFrame; i++)
            {
                GameObject objs = ObjectPoolManager.SpawnObject(obj, player.transform.position, Quaternion.identity);
                objs.GetComponent<objPoolTestObj>().OBM = true;
            }
        }
        else if (objpoolOffTest)
        {
            for(int i = 0; i< repeatNumPerFrame; i++)
            {
                GameObject objs = Instantiate(obj, player.transform.position, Quaternion.identity);
                objs.GetComponent<objPoolTestObj>().OBM = false;
            }
        }
    }
}

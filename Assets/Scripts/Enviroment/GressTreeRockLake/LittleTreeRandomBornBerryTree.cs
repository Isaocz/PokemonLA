using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleTreeRandomBornBerryTree : MonoBehaviour
{

    /// <summary>
    /// 是否真的是树
    /// </summary>
    public bool isRealTree;

    public RandomBerryTree BerryTree;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0.0f, 1.0f) > (MapCreater.StaticMap.IsWailmerPail ? 0.980f : 0.992f)  )
        {
            if (isRealTree || MapCreater.StaticMap.IsWailmerPail)
            {
                if (transform.parent != null)
                {
                    Instantiate(BerryTree, transform.position, Quaternion.identity, transform.parent);
                }
                else
                {
                    Instantiate(BerryTree, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }

}

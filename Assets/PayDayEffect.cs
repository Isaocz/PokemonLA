using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayDayEffect : MonoBehaviour
{

    public Dictionary<int, Empty> enemies;
    private static int PayDayParAmount;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        PayDayParAmount++;
    }
    private void OnDestroy()
    {
        PayDayParAmount--;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(PayDayParAmount > 1 && timer > 1f)
        {
            Destroy(gameObject);
        }
    }
}

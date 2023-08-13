using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleFollow : MonoBehaviour
{
    private PlayerControler player;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 1.5f)
            transform.position = player.transform.position;
    }
}

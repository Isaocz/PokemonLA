using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : Skill
{
    // Start is called before the first frame update
    void Start()
    {
        player.isCanNotMove = true;
        player.isInvincibleAlways = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        player.isCanNotMove = false;
        player.isInvincibleAlways = false;
    }
}

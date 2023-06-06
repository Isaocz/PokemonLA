using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : Skill
{
    // Start is called before the first frame update
    PlayerControler ParentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        ParentPlayer = gameObject.transform.parent.GetComponent<PlayerControler>();
        ParentPlayer.playerData.isMist = true;
    }
    private void Update()
    {
        StartExistenceTimer();
    }

    private void OnDestroy()
    {
        ParentPlayer.playerData.isMist = false;

    }
}

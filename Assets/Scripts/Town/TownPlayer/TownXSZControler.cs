using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownXSZControler : TownPlayer
{
    // Start is called before the first frame update
    void Start()
    {
        Instance();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayer();
    }

    private void FixedUpdate()
    {
        FixedUpdatePlayer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnixRStoneEdge : MonoBehaviour
{

    public OnixStoneEdge StoneEdge;
    public Onix onix;
    public int TurnR;

    float Timer;
    int R;




    // Update is called once per frame
    void Update()
    {
        if (!onix.isEmptyFrozenDone && !onix.isSleepDone && !onix.isCanNotMoveWhenParalysis && !onix.isSilence && !onix.isFearDone) {
            Timer += Time.deltaTime;
            if (Timer >= 0.16f)
            {
                Timer = 0;
                R += -TurnR * (onix.isEmptyConfusionDone ? 120 : 70);
                OnixStoneEdge s1 = Instantiate(StoneEdge, transform.position + Quaternion.AngleAxis(R, Vector3.forward) * Vector3.right * 1.4f, Quaternion.Euler(0, 0, R - (TurnR == 1 ? TurnR : 0) * 180));
                s1.LaunchNotForce(Quaternion.AngleAxis(R + -TurnR * 90, Vector3.forward) * Vector2.right, 8.0f);
                s1.empty = onix;
            }
        }
    }

    private void FixedUpdate()
    {
        float Xmax = int.MinValue; float Xmin = int.MaxValue; float Ymax = int.MinValue; float Ymin = int.MaxValue;
        foreach (OnixBodyShadow b in onix.SubEmptyBodyList)
        {
            if (b.transform.position.x > Xmax) { Xmax = b.transform.position.x; }
            if (b.transform.position.x < Xmin) { Xmin = b.transform.position.x; }
            if (b.transform.position.y > Ymax) { Ymax = b.transform.position.y; }
            if (b.transform.position.y < Ymin) { Ymin = b.transform.position.y; }
        }
        transform.position = new Vector3(Mathf.Clamp( (Xmax + Xmin) / 2.0f , onix.transform.parent.position.x - 12.5f, onix.transform.parent.position.x + 12.5f), Mathf.Clamp((Ymax + Ymin) / 2.0f, onix.transform.parent.position.y - 7.2f, onix.transform.parent.position.y + 7.2f), 0);
    }
}

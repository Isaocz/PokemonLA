using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudShotEffect : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    MudShot ParentMudShot;
    bool isSoftMudBorn;
    public GameObject SoftMud;

    private void Start()
    {
        if (gameObject.transform.parent.GetComponent<MudShot>() != null)
        {
            ParentMudShot = gameObject.transform.parent.GetComponent<MudShot>();
        }
    }


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        
        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (ParentMudShot != null) { ParentMudShot.HitAndKo(target); }
            if (!Empties.Contains(target))
            {
                Empties.Add(target);
                target.SpeedChange();
                target.SpeedRemove01(3 * target.OtherStateResistance);
            }
        }
    }



}

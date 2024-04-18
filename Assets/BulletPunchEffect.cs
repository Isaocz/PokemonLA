using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPunchEffect : MonoBehaviour
{
    Empty target;
    public GameObject TackleBlast;
    BulletPunch ParentBulletPunch;

    private void Start()
    {
        if (gameObject.transform.parent.GetComponent<BulletPunch>() != null)
        {
            ParentBulletPunch = gameObject.transform.parent.GetComponent<BulletPunch>();
        }
    }


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (ParentBulletPunch != null) { ParentBulletPunch.HitAndKo(target); Instantiate(TackleBlast, other.transform.position, Quaternion.identity); }
        }
        else if (other.tag == "Projectel")
        {
            Debug.Log(other);
            Destroy(other.gameObject);

        }
    }
}

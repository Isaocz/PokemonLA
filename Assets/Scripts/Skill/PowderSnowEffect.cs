using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderSnowEffect : MonoBehaviour
{
    Empty target;
    List<Empty> Empties = new List<Empty>();
    bool IsFrozenDone = false;
    public GameObject PowderSnow_02;


    // Start is called before the first frame update
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Empty")
        {
            target = other.GetComponent<Empty>();
            if (!IsFrozenDone)
            {
                if (PowderSnow_02 != null) {
                    Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.0f, 0.3f), 0), Quaternion.identity, target.transform);
                    Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.0f, 0.3f), 0), Quaternion.Euler(0, 0, Random.Range(0, 180)), target.transform);
                    Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.3f, 1.3f), 0), Quaternion.Euler(0, 0, Random.Range(0, 180)), target.transform);
                    Instantiate(PowderSnow_02, target.transform.position + new Vector3(Random.Range(-0.77f, 0.77f), Random.Range(0.3f, 1.3f), 0), Quaternion.Euler(0, 0, Random.Range(0, 180)), target.transform);
                }
                if (Random.Range(0, 9) >= 7)
                {
                    target.Frozen(2.5f , 1);
                    target.playerUIState.StatePlus(2);
                }
            }
            gameObject.transform.parent.GetComponent<MudSlup>().HitAndKo(target);
            IsFrozenDone = true;
        }
    }
}

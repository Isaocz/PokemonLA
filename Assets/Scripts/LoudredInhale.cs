using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudredInhale : MonoBehaviour
{
    public Empty ParentEmpty;
    public float dragForceDefault = 750F;
    public float dragForceEmpty = 100F;
    public float dragForceSubsititue = 7000F;
    public float addForceDis = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other)
    {
        Vector3 direction = Vector3.Normalize(transform.position - other.transform.position);
        if (!ParentEmpty.isEmptyInfatuationDone &&  other.transform.tag == "Player")
        {
            if (Vector3.Distance(transform.position, other.transform.position) > addForceDis)
            {

                PlayerControler playerControler = other.GetComponent<PlayerControler>();
                if (playerControler != null)
                {
                    other.attachedRigidbody.AddForce(dragForceDefault * direction);
                }
                else
                {
                    //ÌæÉí
                    other.attachedRigidbody.AddForce(dragForceSubsititue * direction);
                }
            }
        }
        else if (ParentEmpty.isEmptyInfatuationDone && other.transform.tag == "Empty")
        {
            if (Vector3.Distance(transform.position, other.transform.position) > addForceDis)
            {
                Rigidbody2D rigidbody2D = other.attachedRigidbody;
                if (rigidbody2D)
                {
                    other.attachedRigidbody.AddForce(dragForceEmpty * direction);
                }
            }
        }
    }


}

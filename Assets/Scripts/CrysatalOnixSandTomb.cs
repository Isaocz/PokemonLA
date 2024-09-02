using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrysatalOnixSandTomb : MonoBehaviour
{

    public bool isLarge;
    public float dragForceDefault = 650F;
    public float dragForceSubsititue = 6000F;
    public float addForceDis = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (isLarge)
        {
            Timer.Start(this, 12.0f, () => { Destroy(gameObject); });
        }
        else
        {
            Timer.Start(this, 6.0f, () => { Destroy(gameObject); });
        }
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isLarge) {
            Vector3 direction = Vector3.Normalize(transform.position - other.transform.position);
            if (other.transform.tag == "Player")
            {
                if ((other.gameObject.layer != LayerMask.NameToLayer("PlayerFly") && other.gameObject.layer != LayerMask.NameToLayer("PlayerJump")) && Vector3.Distance(transform.position, other.transform.position) > addForceDis)
                {

                    PlayerControler playerControler = other.GetComponent<PlayerControler>();
                    if (playerControler != null)
                    {
                        other.attachedRigidbody.AddForce(dragForceDefault * direction);
                    }
                    else
                    {
                        //ÃÊ…Ì
                        other.attachedRigidbody.AddForce(dragForceSubsititue * direction);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLarge) {
            if (other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p != null) { p.SpeedChange(); }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isLarge) {
            if (other.tag == "Player")
            {
                PlayerControler p = other.GetComponent<PlayerControler>();
                if (p != null) { p.SpeedRemove01(0); }
            }
        }
    }

}

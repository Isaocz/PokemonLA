using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFunction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectel"))
        {
            Destroy(collision.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BridgeCrossing : MonoBehaviour
{
    [SerializeField] private BoxCollider2D river;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Physics2D.IgnoreCollision(collision, river, true);
            collision.attachedRigidbody.interpolation = RigidbodyInterpolation2D.None;
            collision.transform.parent = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision, river, false);
            collision.attachedRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
            collision.transform.parent = transform.parent;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class BridgeCrossing : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D river;
    private GameObject _playerParent;
    private void Start()
    {
        _playerParent = Instantiate(new GameObject(), transform.position, Quaternion.identity);
    }
    private void Update()
    {
        _playerParent.transform.position = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Physics2D.IgnoreCollision(collision, river, true);
            collision.attachedRigidbody.interpolation = RigidbodyInterpolation2D.None;
            collision.transform.parent = _playerParent.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision, river, false);
            collision.attachedRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
            collision.transform.parent = null;
        }

    }
}

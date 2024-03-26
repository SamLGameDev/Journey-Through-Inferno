using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunge : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartLunge(Rigidbody2D rb, Vector2 direction)
    {
        rb.drag = 100;
        rb.AddForce(direction * 99999 * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

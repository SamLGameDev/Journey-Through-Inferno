using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Steering : MonoBehaviour
{
    [SerializeField] private LayerMask Targets;
    [SerializeField] private float ColliderSize, radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Available_Directions()
    {
            
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 2, Targets);
        foreach (Collider2D c in hit)
        {
            Vector2 direction = c.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distance = direction.magnitude;
            float weight = distance <= ColliderSize ? 1 : (radius - distance) / radius;
            
        }
    }

    void Update()
    {
        Available_Directions();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Furies : BasicAttributes
{
    public LayerMask layerToHit;
    public float shootingRange;
    public float moveSpeed;
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public float shootCooldown = 0f;
}
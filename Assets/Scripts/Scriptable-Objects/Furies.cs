using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/GameObjects/Enemys/Furies")]
public class Furies : BasicAttributes
{
    public float shootingRange;
    public float moveSpeed;
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public float shootCooldown = 0f;
}
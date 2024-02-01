using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class BasicAttributes : ScriptableObject
{
    /// <summary>
    /// the damage the entity does
    /// </summary>
    public int damage;
    /// <summary>
    /// the max health of the entity
    /// </summary>
    public int maxHealth;
    /// <summary>
    /// the card drop chance of the entity
    /// </summary>
    public float cardDropChance;
    /// <summary>
    /// the charge cooldown time before it can charge again
    /// </summary>
}

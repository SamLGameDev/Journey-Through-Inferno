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
    /// the tarot cards it can drop
    /// </summary>
    public List<TarotCards> droppableCards;
    public LayerMask layersToHit;
    public int extraLives;
    public int damageReduction;
    public int armour;
    public int orginalMaxHealth;
    public virtual void Reset()
    {
        maxHealth = orginalMaxHealth;
    }

}

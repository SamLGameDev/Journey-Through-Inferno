using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Player : BasicAttributes
{
    /// <summary>
    /// the cooldown between shots for the gun
    /// </summary>
    public float gunCooldown;
    /// <summary>
    /// the cooldown modifer if the player has the temperence card
    /// </summary>
    public float gunCooldownModifier;
    /// <summary>
    /// speed of the player
    /// </summary>
    public float speed;
    /// <summary>
    /// the speed of of the player with the chariot card
    /// </summary>
    public float chariotSpeed;
    /// <summary>
    /// the speed of the dash
    /// </summary>
    public float dashSpeed;
    /// <summary>
    /// how long the player is dashing
    /// </summary>
    public float dashDuration;
    /// <summary>
    /// how long the cooldwon for the dash is
    /// </summary>
    public float dashCooldown;
    /// <summary>
    /// what layers the  player can hit
    /// </summary>
    public LayerMask layersToHit;
    /// <summary>
    /// the bullet the player shoots
    /// </summary>
    public GameObject bullet;
    /// <summary>
    /// the time until the player goes into the idle animation when standing still
    /// </summary>
    public float timeUntilIdle;
    /// <summary>
    /// the damage of the sword
    /// </summary>
    public int swordDamage;
    /// <summary>
    /// the sword damage with the strength card
    /// </summary>
    public int swordDamageModifier;
    /// <summary>
    /// the speed of the sword swing
    /// </summary>
    public float swordSpeed;
    /// <summary>
    /// how long until they can swing the sword again
    /// </summary>
    public float swordDelay;
    /// <summary>
    /// bullet base damage
    /// </summary>
    public int bulletDamage;
    /// <summary>
    /// the damage the bullet does with the star tarrot card
    /// </summary>
    public int bulletDamageModifier;
    /// <summary>
    /// how fast is the bullet
    /// </summary>
    public float bulletSpeed;
    /// <summary>
    /// how long until the bullet is destoryed
    /// </summary>
    public float bulletLife;
}

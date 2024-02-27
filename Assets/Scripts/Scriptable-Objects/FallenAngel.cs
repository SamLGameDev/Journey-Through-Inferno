using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStats : BasicAttributes
{
    public float chargeCooldown;
    /// <summary>
    /// the the melee range in radius, i.e. 1 will be 1x and 1 y
    /// </summary>
    public float meeleRange;
    /// <summary>
    /// how far away you have to be before the fallen angel will charge
    /// </summary>
    public float chargeFarAway;
    /// <summary>
    /// how close you have to be for the fallen angel to charge
    /// </summary>
    public float chargeNear;
    /// <summary>
    /// the speed of the charge
    /// </summary>
    public float chargeSpeed;
    /// <summary>
    /// how long they will charge for
    /// </summary>
    public float chargeDuration;
    /// <summary>
    /// how long aftter meleeing the entity can meele again
    /// </summary>
    public float chargeDamageInterval;
    /// <summary>
    /// the x size of the meele box
    /// </summary>
    public float meleeSizeX;
    /// <summary>
    /// the y size of the meele box
    /// </summary>
    public float meleeSizeY;
    /// <summary>
    /// how far the boxcast will go
    /// </summary>
    public float meleeDistance;
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

[CreateAssetMenu]
public class Player : BasicAttributes
{
    /// <summary>
    /// the cooldown between shots for the gun
    /// </summary>
    public FloatReference gunCooldown;
    /// <summary>
    /// the cooldown modifer if the player has the temperence card
    /// </summary>
    public FloatReference gunCooldownModifier;
    /// <summary>
    /// speed of the player
    /// </summary>
    public FloatReference speed;
    /// <summary>
    /// the speed of of the player with the chariot card
    /// </summary>
    public FloatReference chariotSpeed;
    /// <summary>
    /// the speed of the dash
    /// </summary>
    public FloatReference dashSpeed;
    /// <summary>
    /// how long the player is dashing
    /// </summary>
    public FloatReference dashDuration;
    /// <summary>
    /// how long the cooldwon for the dash is
    /// </summary>
    public FloatReference dashCooldown;
    /// <summary>
    /// the cooldown between invisibility bursts
    /// </summary>
    public FloatReference invisibilityCooldown;
    /// <summary>
    /// the length of an invisibility burst
    /// </summary>
    public FloatReference invisibilityDuration;
    /// <summary>
    /// the bullet the player shoots
    /// </summary>
    public GameObject bullet;
    /// <summary>
    /// the time until the player goes into the idle animation when standing still
    /// </summary>
    public FloatReference timeUntilIdle;
    /// <summary>
    /// the damage of the sword
    /// </summary>
    public IntReference swordDamage;
    /// <summary>
    /// the sword damage with the strength card
    /// </summary>
    public IntReference swordDamageModifier;
    /// <summary>
    /// the speed of the sword swing
    /// </summary>
    public FloatReference swordSpeed;
    /// <summary>
    /// how long until they can swing the sword again
    /// </summary>
    public FloatReference swordDelay;
    /// <summary>
    /// bullet base damage
    /// </summary>
    public IntReference bulletDamage;
    /// <summary>
    /// the damage the bullet does with the star tarrot card
    /// </summary>
    public IntReference bulletDamageModifier;
    /// <summary>
    /// how fast is the bullet
    /// </summary>
    public FloatReference bulletSpeed;
    /// <summary>
    /// how long until the bullet is destoryed
    /// </summary>
    public FloatReference bulletLife;
    public List<TarotCards> tarotCards;
    public FloatReference criticalHitChance;
    public IntReference criticalHitDamage;
    public Vector3 projectilesize;
    public FloatReference timeUntilSpreadShot;
    public IntReference spreadShotNumber;
    public FloatReference cooldownReduction;
    public BoolReference ControllerRumble;
    public Gamepad gamepad;
    public FloatReference timeUntillRegenAfterAttack;
    public FloatReference timeUntillRegen;
    public int RegenAmount;
    public override void Reset()
    {
        chariotSpeed.value = 0;
        gunCooldownModifier.value = 0;
        swordDamageModifier.value = 0;
        bulletDamageModifier.value = 0;
        tarotCards.Clear();
        extraLives = 0;
        cardDropChance = 0;
        maxHealth = 15;
        swordDamage.value = 4;
        bulletDamage.value = 3;
        criticalHitChance.value = 0;
        damageReduction = 0;
        armour = 0;
        projectilesize = new Vector3(1,1,1);
        spreadShotNumber.value = 0;
        cooldownReduction.value = 0;
        droppableCards.Clear();
        RegenAmount = 0;
    }
}

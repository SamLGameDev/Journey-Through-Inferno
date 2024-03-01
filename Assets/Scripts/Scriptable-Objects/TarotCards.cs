
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class TarotCards :ScriptableObject
{
    public Sprite cardImage;
    public Player players;
    public int effectValue;
    [Multiline]
    public string description;
    public bool specialCardEffectEnabled;
    public float RangeForAbility;
    public GameObject particleEffect;
    public possibleModifiers possibleMods;
    public enum possibleModifiers 
    {
        None,
        GunDamage,
        SwordDamge,
        speed,
        guncooldown,
        ExplodingEnemies,
        TwoCards,
        increasedDropChance,
        extraLife,
        IncreasedDamageLowerHealth,
        criticalHit,
        damageReduction,
        increaseMaxHealth,
        armour,
        increaseProjectileSize,
        SpreadShot,
        reducedBossHealth,
        Homing
    }
    public void ApplyEffect(GameObject p)
    {
        if (specialCardEffectEnabled)
        {
            return;
        }
        Player stats = p.GetComponent<Player_movement>().stats;
        switch (possibleMods)
        {
            case possibleModifiers.None:
                return;
            case possibleModifiers.GunDamage:
                stats.bulletDamageModifier = effectValue;
                break;
            case possibleModifiers.SwordDamge:
                stats.swordDamageModifier = effectValue;
                break;
            case possibleModifiers.speed:
                stats.chariotSpeed = effectValue;
                p.GetComponent<Player_movement>().UpdateSpeed();
                break;
            case possibleModifiers.guncooldown:
                stats.gunCooldownModifier = effectValue;
                break;
            case possibleModifiers.increasedDropChance:
                stats.cardDropChance = effectValue;
                break;
            case possibleModifiers.extraLife:
                stats.extraLives = effectValue;
                break;
            case possibleModifiers.IncreasedDamageLowerHealth:
                stats.maxHealth -= effectValue;
                stats.swordDamage += (int)RangeForAbility;
                stats.bulletDamage += (int)RangeForAbility;
                break;
            case possibleModifiers.criticalHit:
                stats.criticalHitChance = effectValue;
                break;
            case possibleModifiers.damageReduction:
                stats.damageReduction = effectValue;
                break;
            case possibleModifiers.increaseMaxHealth:
                stats.maxHealth += effectValue;
                break;
            case possibleModifiers.armour:
                stats.armour = effectValue;
                break;
            case possibleModifiers.increaseProjectileSize:
                stats.projectilesize = new Vector3(effectValue, effectValue, 0);
                break;
            case possibleModifiers.SpreadShot:
                stats.spreadShotNumber = effectValue;
                break;
        }
    }

}

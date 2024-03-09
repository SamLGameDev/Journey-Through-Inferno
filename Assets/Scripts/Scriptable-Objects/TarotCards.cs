
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class TarotCards :ScriptableObject
{
    public Sprite cardImage;
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
        ExplodingEnemies,
        TwoCards,
        increasedDropChance,
        extraLife,
        IncreasedDamageLowerHealth,
        criticalHit,
        damageReduction,
        increaseMaxHealth,
        HealthRegen,
        increaseProjectileSize,
        SpreadShot,
        reducedBossHealth,
        Homing,
        invisibility,
        cooldownReduction
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
                stats.bulletDamageModifier.value = effectValue;
                break;
            case possibleModifiers.SwordDamge:
                stats.swordDamageModifier.value = effectValue;
                break;
            case possibleModifiers.speed:
                stats.chariotSpeed.value = effectValue;
                p.GetComponent<Player_movement>().UpdateSpeed();
                break;
            case possibleModifiers.increasedDropChance:
                stats.cardDropChance = effectValue;
                break;
            case possibleModifiers.extraLife:
                stats.extraLives = effectValue;
                break;
            case possibleModifiers.IncreasedDamageLowerHealth:
                stats.maxHealth -= effectValue;
                stats.swordDamage.value += (int)RangeForAbility;
                stats.bulletDamage.value += (int)RangeForAbility;
                break;
            case possibleModifiers.criticalHit:
                stats.criticalHitChance.value = effectValue;
                break;
            case possibleModifiers.damageReduction:
                stats.damageReduction = effectValue;
                break;
            case possibleModifiers.increaseMaxHealth:
                stats.maxHealth += effectValue;
                break;
            case possibleModifiers.HealthRegen:
                stats.RegenAmount = effectValue;
                break;
            case possibleModifiers.increaseProjectileSize:
                stats.projectilesize = new Vector3(effectValue, effectValue, 0);
                break;
            case possibleModifiers.SpreadShot:
                stats.spreadShotNumber.value = effectValue;
                break;
            case possibleModifiers.cooldownReduction:
                stats.cooldownReduction.value = effectValue;
                stats.gunCooldownModifier.value = effectValue;
                break;
        }
    }

}

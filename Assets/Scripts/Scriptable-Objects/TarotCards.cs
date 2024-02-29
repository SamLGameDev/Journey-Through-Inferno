
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
        increasedDropChance
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
        }
    }

}

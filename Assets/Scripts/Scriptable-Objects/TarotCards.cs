using Microsoft.Unity.VisualStudio.Editor;
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
    public string description;
    public bool specialCardEffectEnabled;
    public possibleModifiers possibleMods;
    public enum possibleModifiers 
    {
        None,
        GunDamage,
        SwordDamge,
        speed,
        guncooldown,
    }


}

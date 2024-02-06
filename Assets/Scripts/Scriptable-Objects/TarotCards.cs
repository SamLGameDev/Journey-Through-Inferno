using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TarotCards :ScriptableObject
{
    public Image cardImage;
    public int effectValue;
    public string description;
    public bool specialCardEffectEnabled;

}

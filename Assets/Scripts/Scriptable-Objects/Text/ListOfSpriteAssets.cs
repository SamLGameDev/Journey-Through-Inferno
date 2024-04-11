using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
[CreateAssetMenu(menuName = "ScriptableObjects/DataHolders/TMPSpriteList")]
public class ListOfSpriteAssets : ScriptableObject
{
    public List<TMP_SpriteAsset> assest;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "ScriptableObjects/RunTimeSets/GameObjectCounter")]
public class GameObjectCounter : Counter<GameObject>
{
   public override void Add(GameObject t)
    {
        if (!Items.Contains(t)) { Items.Add(t); }
        Items.Sort((a, b) => string.Compare(a.name, b.name));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/GameObjects/Enemys/Pluto")]
public class Pluto : BasicAttributes
{
    public int moveInterval;
    public int cerberusCooldown;
    public float cerberusRange;
    public float cerberusSpeed;
    public float statueDelay;
    public GameObject cerberusPrefab;
    public GameObject statuePrefab;
    public GameObject clonePrefab;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/GameObjects/Enemys/Pluto")]
public class Pluto : BasicAttributes
{
    public float moveInterval;
    public float cerberusCooldown;
    public float cerberusSpeed;
    public float statueDelay;
    public GameObject cerberusPrefab;
    public GameObject statuePrefab;
    public GameObject clonePrefab;
}

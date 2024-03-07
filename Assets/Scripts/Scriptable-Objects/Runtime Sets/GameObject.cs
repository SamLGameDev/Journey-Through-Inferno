using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameObjectCounter : Counter<GameObject> 
{


    public override void Remove(GameObject t)
    {
        base.Remove(t);
    }
 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTriggered : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<SpikeFall>().SetState = SpikeFall.State.prefall;
    }

}

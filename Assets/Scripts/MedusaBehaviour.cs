using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaBehaviour : MonoBehaviour
{
    private enum BossState
    {
        Melee,
        Ranged
    }

    private BossState currentState;

    private void Start()
    {
        currentState = BossState.Melee;
    }

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        
    }
}

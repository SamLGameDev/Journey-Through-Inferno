using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaBehaviour : MonoBehaviour
{
    [SerializeField] private float minTimeBetweenPosChange;
    [SerializeField] private float maxTimeBetweenPosChange;

    [SerializeField] private float meleeRange;

    private GameObject[] players;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        players = GameObject.FindGameObjectsWithTag("Player");

        CheckMeleeRange();
    }
    
    public void CheckMeleeRange()
    {
        if (animator.GetBool("inMeleeRange") == true)
        {
            return;
        }

        foreach (GameObject player in players)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < meleeRange)
            {
                animator.SetBool("inMeleeRange", true);
            }
        }
    }
}

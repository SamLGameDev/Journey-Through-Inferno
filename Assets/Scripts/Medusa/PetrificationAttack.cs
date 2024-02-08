using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrificationAttack : MonoBehaviour
{
    [HideInInspector]
    public float petrifyTime;

    [HideInInspector]
    public Transform medusa;

    public float petrifyProgress;

    [HideInInspector]
    public bool petrified;

    // Start is called before the first frame update
    void Start()
    {
        petrifyProgress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!medusa.GetComponent<MedusaBehaviour>().attacking)
        {
            return;
        }

        if (petrified)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, medusa.position - transform.position);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            petrifyProgress += Time.deltaTime;

            if (petrifyProgress >= petrifyTime)
            {
                petrified = true;

                // Trigger petrify. 
                GetComponent<Player_movement>().enabled = false;
                GetComponent<SpriteRenderer>().color = Color.gray;
                GetComponent<Animator>().speed = 0f;

                // Start a countdown to unpetrify the player.
                MedusaBehaviour mb = medusa.GetComponent<MedusaBehaviour>();
                mb.StartCoroutine(mb.Unfreeze(gameObject, this));

                petrifyProgress = 0;
            }
        }
    }


}

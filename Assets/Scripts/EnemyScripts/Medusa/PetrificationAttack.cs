using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrificationAttack : MonoBehaviour
{
    [HideInInspector]
    public float petrifyTime;

    [HideInInspector]
    public Transform medusa;

    private GameObject petrificationParticles;

    public float petrifyProgress;

    [HideInInspector]
    public bool petrified;

    public bool isPetrified = false;

    // Start is called before the first frame update
    void Start()
    {
        petrifyProgress = 0;

        petrificationParticles = medusa.GetComponent<MedusaBehaviour>().petrificationParticles;
    }

    // Update is called once per frame
    void Update()
    {
        if (medusa == null)
        {
            return;
        }
        if (!medusa.GetComponent<MedusaBehaviour>().attacking)
        {
            return;
        }

        if (petrified)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, medusa.position - transform.position, Mathf.Infinity);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            petrifyProgress += Time.deltaTime;

            Debug.DrawLine(transform.position, hit.collider.transform.position);

            if (petrifyProgress >= petrifyTime)
            {
                petrified = true;

                // Trigger petrify. 
                GetComponent<Player_movement>().enabled = false;
                GetComponent<SpriteRenderer>().color = Color.gray;
                GetComponent<Animator>().speed = 0f;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                isPetrified = true;

                //Play paricles.
                Instantiate(petrificationParticles, transform.position, Quaternion.identity);

                // Start a countdown to unpetrify the player.
                MedusaBehaviour mb = medusa.GetComponent<MedusaBehaviour>();
                mb.StartCoroutine(mb.Unfreeze(gameObject, this));

                petrifyProgress = 0;
            }
        }
    }


}

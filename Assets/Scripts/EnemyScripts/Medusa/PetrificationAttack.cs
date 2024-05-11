using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public bool beenPetrified = false;

    private float TimeWhenPetrified;

    // Start is called before the first frame update
    void Start()
    {
        petrifyProgress = 0;

        petrificationParticles = medusa.GetComponent<MedusaBehaviour>().petrificationParticles;
    }

    // Update is called once per frame
    void Update()
    {
        if (beenPetrified && Time.time - 4 > TimeWhenPetrified)
        {
            GetComponent<Player_movement>().Unfreeze(this);
        }
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
            GetComponent<Animator>().speed = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, medusa.position - transform.position, Mathf.Infinity);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            petrifyProgress += Time.deltaTime;

            Debug.DrawLine(transform.position, hit.collider.transform.position);

            if (petrifyProgress >= petrifyTime && !beenPetrified)
            {
                petrified = true;
                beenPetrified = true;

                // Disable input.
                GetComponent<Player_movement>().InputManager.CutsceneStarted();
                GetComponent<Player_movement>().isPetrified = true;


                // Set color to petrified effect.
                GetComponent<SpriteRenderer>().color = Color.gray;

                // Freeze the player.
                GetComponent<Animator>().speed = 0f;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                isPetrified = true;

                //Play paricles.
                Instantiate(petrificationParticles, transform.position, Quaternion.identity);

                // Start a countdown to unpetrify the player.
                

                TimeWhenPetrified = Time.time;

                petrifyProgress = 0;
            }

        }

    }

    private void OnDestroy()
    {
        //medusa.GetComponent<MedusaBehaviour>().Unfreeze(gameObject, this);
    }
}

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class Different_Moves : MonoBehaviour
{
    /// <summary>
    /// variables for the timer between gun shots
    /// </summary>
    public float coolDown;
    private float shootTimer = 0;
    /// <summary>
    /// the Layers that you want to hit with the attacks. set in the inspector
    /// </summary>
    public LayerMask LayersToHit;
    /// <summary>
    /// the speed for the sword swing
    /// </summary>
    public float sword_Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// a Melee attack that contains a float for y range, int for damage
    /// </summary>
    /// <param name="Melee_info"></param>
    public virtual void Melee(AnimationEvent Melee_info)
    {
        // creates a box that will grab everything with a collider in it that matches the layers to
        // hit then apply damage to it.
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x, Melee_info.floatParameter), transform.rotation.z, Vector2.up, Melee_info.floatParameter, LayersToHit);
        foreach (RaycastHit2D hit2d in hit)
        {
            hit2d.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(Melee_info.intParameter);
        }


    }
    /// <summary>
    /// a Melee attack that contains a float for y range, int for damage, float for cooldown before melee again
    /// </summary>
    /// <param name="range"></param>
    /// <param name="damage"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public virtual float Melee(float range, int damage, float time)
    {
        // a delay to stop the player from spamming melee
        if (Time.time - 0.5 < time)
        {
            return time;
        }
        // same as previous melee function this is an override to
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x + 1, range), 0 , transform.right, range, LayersToHit);
        foreach (RaycastHit2D hit2d in hit)
        {
            hit2d.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(damage);
        }
        return Time.time;


    }


    /// <summary>
    /// Enables A* pathfinding components. Does not disable the A* graph itself, just destination setters.
    /// </summary>
    /// <param name="setter"></param>
    private void EnablerAStar(bool setter)
    {
        GetComponent<Seeker>().enabled = setter;
        GetComponent<AIDestinationSetter>().enabled = setter;
        GetComponent<AIPath>().enabled = setter;
    }
    /// <summary>
    /// a charge attack that moves the object in the position it was facing for 2 seconds
    /// </summary>
    /// <param name="Charge_info"></param>
    /// <returns></returns>
    public IEnumerator Charge(AnimationEvent Charge_info)
    {
        // stops the animation
        GetComponent<Animator>().SetBool("Within_Charge_Range", false);
        // gets the cooldown time for the melee so its not istantly killing whatever it touches 
        float Melee_Damage_Cooldown = -5;
        float time = Time.time;
        // duration od the charge
        while (Time.time - 2 < time)
        {
            yield return null;
            // move it in the direction it is facing at a speed if 5
            transform.position += transform.right * Time.deltaTime * 5;
            // calles melee to get new cooldown or attack
            Melee_Damage_Cooldown = Melee(5, 6, Melee_Damage_Cooldown);
        }
        // reenables pathfinding so thta it can move again
        EnablerAStar(true);
    }
    /// <summary>
    /// constantly rotates the objetc so it is facing its target
    /// </summary>
    /// <returns></returns>
    public IEnumerator LookAtEnemy()
    {
        // while the animation is playing
        while (GetComponent<Animator>().GetBool("Within_Charge_Range") == true)
        {
            yield return null;
            Transform target = GetComponent<AIDestinationSetter>().target;
            // if its target dies
            if (!target)
            {
                break;
            }
            // get the difference in thier positions then rotate it acording to that after its been Atan2 and Rad2deg
            Vector3 difference = target.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
    }
    /// <summary>
    /// calculates the limit of where the sword should reach before being reset based on the facing direction of the player
    /// </summary>
    /// <returns></returns>
    private float calculate_limit()
    {
        Player_movement player = GetComponent<Player_movement>();
        float limit;
        // if the player is facing up or down
        if (!player.upDown)
        {
            // -1 is moving to the right
            limit = player.facing == -1 ? -0.7f : 0.7f;
        }
        // right or left
        else
        {
            limit = player.facing == -1 ? -0.999f : 0.999f;
        }
        return limit;
    }
    /// <summary>
    /// rotates the sword around the player based on the facing direction and resets to the start when it reches halfway
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator RotateAround(GameObject target)
    {
        Player_movement player = GetComponent<Player_movement>();
        float limit = calculate_limit();
        // the fakelimit is for when it is facing downwards as it needs conditions that
        // would break the rest of the program if used with limit
        float fakelimit = -2;
        // if facing downwards
        if (limit == 0.7f)
        {
            fakelimit = 0.7f;
            limit = -2; // -2 so it doesnt trigger anything
        }
        int facing = player.facing;
        Quaternion rotation = target.transform.rotation; // get the original rotation
        Vector2 position = target.transform.localPosition; // get the original position

        while (GetComponent<Player_movement>().running) // while the attack button has been pressed
        {
            // rotate it around the player based ont he facing direction
            target.transform.RotateAround(transform.position, new Vector3(0, 0, facing), sword_Speed * Time.deltaTime);
            // handles the halfway break for both left right down and up, fake being for down
            if ((target.transform.rotation.z <= limit && limit < 0) || (target.transform.rotation.z >= limit && limit > 0) || (target.transform.rotation.z <= fakelimit && fakelimit > 0))
            {
                target.transform.rotation = rotation; // to account for any chnages in rotation to set the sword straight
                target.transform.localPosition = position; // sets its position to where it started
                GetComponent<Player_movement>().running = false;
                target.SetActive(false); // make the sword invisible and stop damage when not attacking
                yield return new WaitUntil(() => GetComponent<Player_movement>().running); // wait unitll attacking again 
                // recalculate everything for the new attack
                facing = player.facing;
                limit = calculate_limit();
                fakelimit = -2;
                if (limit == 0.7f)
                {
                    fakelimit = 0.7f;
                    limit = -2;
                }
                rotation = target.transform.rotation;
                position = target.transform.localPosition;
            }
            yield return null;
        }
    }
    /// <summary>
    /// shoots a beam that upon coliding with an enemy will make it take damage or a mirror will reflect it
    /// </summary>
    /// <param name="target"></param>
    /// <param name="originalHit"></param>
    /// <param name="angle"></param>
    public void Shoot(LayerMask target, Vector2 originalHit, Vector2 angle)
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer > 0)
        {
            return;
        }

        bool isMirror = false;
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(originalHit.x, originalHit.y) , angle, 1000, target);
        Debug.DrawRay(originalHit, angle * 30, Color.red, 0.1f);

        if (hit && hit.collider.CompareTag("Mirror") && !isMirror)
        {
            // gets the angle of the reflection
            var reflectangle = Vector2.Reflect(hit.point, hit.normal);
            // calles shoot again but with the new beam shoudl be reflected from and new angle
            isMirror = true;
            Shoot(target, hit.point, reflectangle);
        }


        if (hit && hit.collider.CompareTag("Enemy"))
        {
            hit.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(5);
        }

        shootTimer = coolDown;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

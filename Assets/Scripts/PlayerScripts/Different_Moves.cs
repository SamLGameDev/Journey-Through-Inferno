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
    public LayerMask LayersToHitWhenConfused;
    /// <summary>
    /// the speed for the sword swing
    /// </summary>
    public float sword_Speed;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private Player pStats;
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
        Debug.Log("being called");
        RaycastHit2D[] hit = Physics2D.BoxCastAll(new Vector2(transform.position.x, transform.position.y + 1 + (stats.meleeSizeY / 2)), new Vector2(stats.meleeSizeX, stats.meleeSizeY), 0, transform.up, stats.meleeDistance,
            GetComponent<EntityHealthBehaviour>().Confused ? LayersToHitWhenConfused : stats.layersToHit);
        foreach (RaycastHit2D hit2d in hit)
        {
            if (hit2d.collider.GetComponent<EntityHealthBehaviour>() == null) { return; }
            hit2d.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.damage);
            Debug.Log("has Hit");
        }


    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y + 1 + (stats.meleeSizeY / 2)), new Vector2(stats.meleeSizeX, stats.meleeSizeY));
    }
    /// <summary>
    /// a Melee attack that contains a float for y range, int for damage, float for cooldown before melee again
    /// </summary>
    /// <param name="range"></param>
    /// <param name="damage"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public virtual float Melee(float time)
    {
        // a delay to stop the player from spamming melee
        if (Time.time - stats.chargeDamageInterval < time)
        {
            return time;
        }
        // same as previous melee function this is an override to
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(stats.meleeSizeX, stats.meleeSizeY), 0 , transform.up, stats.meleeDistance, GetComponent<EntityHealthBehaviour>().Confused ? LayersToHitWhenConfused : stats.layersToHit);

        foreach (RaycastHit2D hit2d in hit)
        {
            if (hit2d.collider.GetComponent<EntityHealthBehaviour>() != null)
            {
                hit2d.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.damage);
            }
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // duration od the charge
        rb.AddForce(transform.up * stats.chargeSpeed);
        while (Time.time - stats.chargeDuration < time)
        {
            yield return null;
            // move it in the direction it is facing at a speed if 5

            // calles melee to get new cooldown or attack
            Melee_Damage_Cooldown = Melee(Melee_Damage_Cooldown);
        }

        rb.velocity = Vector3.zero;
        // reenables pathfinding so that it can move again
        GetComponent<Range_Calculator>().state = Range_Calculator.Angel_State.normal;
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
            transform.up = target.position - transform.position;
        }
    }

    //private void SpawnSwordBeam()
    //{
    //    Instantiate(GameManager.instance.bullet, transform);
    //}
    /// <summary>
    /// shoots a beam that upon coliding with an enemy will make it take damage or a mirror will reflect it
    /// </summary>
    /// <param name="target"></param>
    /// <param name="originalHit"></param>
    /// <param name="angle"></param>
    public void Shoot(LayerMask target, Vector2 originalHit, Vector2 angle, GameObject bullet, bool confusion)
    {
        #region old stuff
        //shootTimer -= Time.deltaTime;
        //if (shootTimer > 0)
        //{
        //    return;
        //}

        //bool isMirror = false;
        //Physics2D.queriesStartInColliders = false;
        //RaycastHit2D hit = Physics2D.Raycast(new Vector2(originalHit.x, originalHit.y) , angle, 1000, target);
        //Debug.DrawRay(originalHit, angle * 30, Color.red, 0.1f);

        //if (hit && hit.collider.CompareTag("Mirror") && !isMirror)
        //{
        //    // gets the angle of the reflection
        //    var reflectangle = Vector2.Reflect(hit.point, hit.normal);
        //    // calles shoot again but with the new beam shoudl be reflected from and new angle
        //    isMirror = true;
        //    Shoot(target, hit.point, reflectangle);
        //}


        //if (hit && hit.collider.CompareTag("Enemy"))
        //{
        //    hit.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(5);
        //}

        //shootTimer = coolDown;
        #endregion old stuff
        GameObject projectile;
        // instanciates a bullet in the facing direction of the compass
        if (confusion)
        {
            projectile = Instantiate(pStats.confusionBullet, transform.position, transform.GetChild(0).GetChild(0).rotation);
            projectile.GetComponent<ConfusionBulletController>().stats = pStats;
        }
        else
        {
            projectile = Instantiate(bullet, transform.position, transform.GetChild(0).GetChild(0).rotation);
            projectile.GetComponent<bullet_controller>().stats = pStats;
        }
        projectile.transform.parent = transform;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

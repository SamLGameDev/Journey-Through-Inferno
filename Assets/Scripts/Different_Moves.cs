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
    private float damageDelay = 0;
    private Coroutine Player_Coroutine;
    
    public LayerMask LayersToHit;
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

        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x, Melee_info.floatParameter), transform.rotation.z, Vector2.up, Melee_info.floatParameter, LayersToHit);
        foreach (RaycastHit2D hit2d in hit)
        {
            hit2d.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(Melee_info.intParameter);
        }


    }
    public virtual float Melee(float range, int damage, float time)
    {
        if (Time.time - 0.5 < time)
        {
            return time;
        }
        
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x + 1, range), 0 , transform.right, range, LayersToHit);
        foreach (RaycastHit2D hit2d in hit)
        {
            hit2d.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(damage);
        }
        return Time.time;


    }



    private void EnablerAStar(bool setter)
    {
        GetComponent<Seeker>().enabled = setter;
        GetComponent<AIDestinationSetter>().enabled = setter;
        GetComponent<AIPath>().enabled = setter;
    }
    public IEnumerator Charge(AnimationEvent Charge_info)
    {
        GetComponent<Animator>().SetBool("Within_Charge_Range", false);
        float Melee_Damage_Cooldown = -5;
        //Vector2 target = Calculate_Direction();
        float time = Time.time;
        while (Time.time - 2 < time)
        {
            yield return null;
            transform.position += transform.right * Time.deltaTime * 5;
            Melee_Damage_Cooldown = Melee(5, 6, Melee_Damage_Cooldown);
        }
        EnablerAStar(true);
    }
    public IEnumerator LookAtEnemy()
    {
        while (GetComponent<Animator>().GetBool("Within_Charge_Range") == true)
        {
            yield return null;
            Transform target = GetComponent<AIDestinationSetter>().target;
            if (!target)
            {
                break;
            }
            Vector3 difference = target.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        }
    }
    public void Player_Sword_Attack(int facing)
    {
        GameObject sword = transform.GetChild(1).gameObject;
        sword.SetActive(true);
        float time = Time.time;
        GetComponent<Player_movement>().running = false;
        Player_Coroutine = StartCoroutine(RotateAround(sword, time, facing));
        

 
    }
    private IEnumerator RotateAround(GameObject target, float deltaTime, int facing)
    {
        float limit = -0.9999f;
        while (!GetComponent<Player_movement>().actions.FindAction("Actions").triggered || GetComponent<Player_movement>().running)
        {
            yield return null;
        }
        GetComponent<Player_movement>().running = true;
        while (Time.time - 10 < deltaTime)
        {
            if (target.transform.rotation.z <= limit || target.transform.rotation.z > 0)
            {
                target.transform.RotateAround(transform.position, new Vector3(0, 0, -facing), 180);
                GetComponent<Player_movement>().running = false;
                Player_Coroutine.reset
                yield break;
            }
            target.transform.RotateAround(transform.position, new Vector3(0,0,facing), 260 * Time.deltaTime);
            yield return null;
        }
    }

    public void Shoot(LayerMask target, Vector2 originalHit, Vector2 angle)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(originalHit.x + 0.3f, originalHit.y + 0.3f) , angle, 1000, target);
        Color red = Color.red;
        Debug.DrawRay(originalHit, angle * 30, red);

        if (hit && hit.collider.CompareTag("Mirror"))
        {
            
            var reflectangle = Vector2.Reflect(transform.right, hit.normal);
            Shoot(target, hit.point, reflectangle);
        }

        damageDelay -= Time.deltaTime;
        if (damageDelay <= 0)
        {
            if (hit && hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(1);
            }
            damageDelay = 0.05f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

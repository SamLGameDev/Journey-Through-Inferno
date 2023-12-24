using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using static UnityEngine.GraphicsBuffer;

public class Different_Moves : MonoBehaviour
{
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
            return Time.time;
        }

        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x, range), transform.rotation.z, Vector2.up, range, LayersToHit);
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
    private Vector2 Calculate_Direction()
    {
        if (transform.rotation.z > 360)
        {
            transform.rotation = new Quaternion(0,0, 0, 0); 
        }
        if (transform.rotation.z >= -90 && transform.rotation.z < 0)
        { 
            float percent = -transform.rotation.z / 90;
            float remaining = 1 - percent;
            return new Vector2(transform.position.x * percent, transform.position.y * remaining);
        }
        else if (transform.rotation.z < -90 && transform.rotation.z >= -180)
        {
            float percent = (-transform.rotation.z-90) / 90;
            float remaining = 1 - percent;
            return new Vector2(transform.position.x * remaining, -transform.position.y * percent);
        }
        if (transform.rotation.z >= -270 && transform.rotation.z < -180)
        {
            float percent = (-transform.rotation.z-180) / 90;
            float remaining = 1 - percent;
            return new Vector2(- -transform.position.x * percent, -transform.position.y * remaining);
        }
        else if (transform.rotation.z < -270 && transform.rotation.z >= -360)
        {
            float percent = (-transform.rotation.z-270) / 90;
            float remaining = 1 - percent;
            return new Vector2(transform.position.x * remaining, -transform.position.y * percent);
        }

        return new Vector2(transform.position.x, -transform.position.y);    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

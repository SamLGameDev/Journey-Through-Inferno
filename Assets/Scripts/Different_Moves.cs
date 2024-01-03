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

        if (hit && hit.collider.CompareTag("Enemy"))
        {
            hit.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(5);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

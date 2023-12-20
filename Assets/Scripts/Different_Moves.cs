using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Melee(AnimationEvent Melee_info)
    {

        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x, Melee_info.floatParameter) ,transform.rotation.z, Vector2.up, Melee_info.floatParameter, LayersToHit);
        foreach (RaycastHit2D hit2d in hit)
        {
            hit2d.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(Melee_info.intParameter);
        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

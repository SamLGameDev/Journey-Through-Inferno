using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Different_Moves : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// a Melee attack that contains a float for y range, int for damage, string for layer to target
    /// </summary>
    /// <param name="Melee_info"></param>
    public void Melee(AnimationEvent Melee_info)
    {
        
        LayerMask targetLayer = LayerMask.NameToLayer(Melee_info.stringParameter);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(transform.localScale.x, Melee_info.floatParameter) ,transform.rotation.z, Vector2.up, Melee_info.floatParameter , targetLayer.value);
        Debug.Log(hit.collider.gameObject.name);
        hit.collider.gameObject.GetComponent<EntityHealthBehaviour>().ApplyDamage(Melee_info.intParameter);


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

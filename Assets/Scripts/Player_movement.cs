using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private float speed;
    private InputActionAsset actions;
    private float delay = -0.05f;
    private Different_Moves moves;
    [SerializeField] private LayerMask GunTargets;
    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
        moves=  GetComponent<Different_Moves>();
    }
    public void Joystic_Movement()
    {
        // Gets the movement action and moves the player based on that times speed
       Vector2 movement = actions.FindAction("Movement").ReadValue<Vector2>();
       GetComponent<Rigidbody2D>().velocity = new Vector2(movement.x * speed, movement.y * speed);
       float heading  = Mathf.Atan2(movement.x, movement.y);
       transform.rotation = new Quaternion(0,0, heading *Mathf.Rad2Deg, 0);
    }
    private void Player_Melee()
    {
        float time = moves.Melee(0.6f, 2, delay);
        delay = time;

    }
    private void Player_Shooting()
    {
        moves.Shoot(GunTargets, transform.position, transform.right);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (actions.FindAction("Actions").IsPressed())
        {
              Player_Melee();
        }
        if (actions.FindAction("Shoot").IsPressed())
        {
            Player_Shooting();
        }
        Joystic_Movement();
    }
}

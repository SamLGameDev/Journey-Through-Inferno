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
    private Rigidbody2D rb;
    private Animator ani;
    private float time;
    private bool VelocityCheck;
    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
        moves=  GetComponent<Different_Moves>();
        rb = GetComponent<Rigidbody2D>();   
        ani = GetComponent<Animator>();
    }
    public void Joystic_Movement()
    {
        // Gets the movement action and moves the player based on that times speed
       Vector2 movement = actions.FindAction("Movement").ReadValue<Vector2>();
       GetComponent<Rigidbody2D>().velocity = new Vector2(movement.x * speed, movement.y * speed);
       Vector2 GetRotation = actions.FindAction("Aim").ReadValue<Vector2>();
       float heading  = Mathf.Atan2(GetRotation.x, -GetRotation.y);
       transform.GetChild(0).rotation = Quaternion.Euler(0,0, heading * Mathf.Rad2Deg);
       
    }
    private void Animation_Controller()
    {
        Vector2 movement = actions.FindAction("Movement").ReadValue<Vector2>();
        float velo = Mathf.Abs(rb.velocity.x + rb.velocity.y);
        ani.SetFloat("Velocity",velo);
        if (velo < 0.0001  && !VelocityCheck) 
        {
            time = Time.time;
            VelocityCheck = true;
            return;
        }
        Debug.Log(Mathf.Abs(rb.velocity.y));
        if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x))
        {
            Debug.Log(rb.velocity.y);
            if (rb.velocity.y < 0)
            {
                Debug.Log("y>x");
                ani.SetBool("Y>X", true);
                ani.SetBool("Positive Y>X change", false);
            }
            else
            {
                ani.SetBool("Y>X", false);
                ani.SetBool("Positive Y>X change", true);
            }
        }
        else
        {
            if (rb.velocity.x < 0)
            {
                Debug.Log("left");
                ani.SetBool("Negative x", true);
            }
            else
            {
                Debug.Log("right");
                ani.SetBool("Negative x", false);
            }
            ani.SetBool("Y>X", false);
            ani.SetBool("Positive Y>X change", false);
        }
    }
    public void Flip_Xaxis()
    {
        GetComponent<SpriteRenderer>().flipX = true;
    }
    public void UnFlip_Xaxis()
    {
        GetComponent<SpriteRenderer>().flipX = false;
    }
    private void Player_Melee()
    {
        float time = moves.Melee(1.6f, 2, delay);
        delay = time;

    }
    private void Player_Shooting()
    {
        moves.Shoot(GunTargets, transform.position, transform.GetChild(0).GetChild(0).right);
    }
    private void IdleCheck()
    {
        float velo = Mathf.Abs(rb.velocity.x + rb.velocity.y);
        Debug.Log("pass 1");
        if (velo < 0.0001)
        {
            Debug.Log("pass 2");
            if (Time.time - 3 > time)
            {
                ani.SetBool("Time passed 5", true);
                Debug.Log("pass 3");
            }
            return;
        }
        else
        {
            VelocityCheck = false;
            ani.SetBool("Time passed 5", false);
            return;
        }
        
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
        if (VelocityCheck) 
        {
            IdleCheck();
        }
        Animation_Controller();
        Joystic_Movement();
    }
}

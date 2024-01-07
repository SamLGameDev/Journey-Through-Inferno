using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [NonSerialized] public InputActionAsset actions;
    private Different_Moves moves;
    [SerializeField] private LayerMask GunTargets;
    private Rigidbody2D rb;
    private Animator ani;
    private float time;
    private bool VelocityCheck;
    public int facing = -1;
    public bool upDown = true;
    public bool running=  false;
    bool passed = false;
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
        if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x))
        {
            if (upDown && !running)
            {
                RotateAround(1);
                upDown = false;
            }
            if (rb.velocity.y < 0)
            {
                ani.SetBool("Y>X", true);
                ani.SetBool("Positive Y>X change", false);
                facing = 1;
            }
            else if (rb.velocity.y > 0) 
            {
                ani.SetBool("Y>X", false);
                ani.SetBool("Positive Y>X change", true);
                facing = -1;
            }
            else
            {
                ani.SetBool("Y>X", false);
                ani.SetBool("Positive Y>X change", false);
            }
        }
        else if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(rb.velocity.x))
        {
            if (!upDown && !running)
            {
                RotateAround(-1);
                upDown = true;
            }
            if (rb.velocity.x < 0)
            {
                ani.SetBool("Negative x", true);
                facing = 1;
            }
            else if (rb.velocity.x > 0)
            {
                ani.SetBool("Negative x", false);
                facing = -1;
            }
            ani.SetBool("Y>X", false);
            ani.SetBool("Positive Y>X change", false);
        }
    }
    private void RotateAround(int direction)
    {
        GameObject target = transform.GetChild(1).gameObject;
        target.transform.RotateAround(transform.position, new Vector3(0, 0, direction), 90);
        
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
        //float time = moves.Melee(1.6f, 2, delay);
        //delay = time;
        moves.Player_Sword_Attack();

    }
    private void Player_Shooting()
    {
        moves.Shoot(GunTargets, transform.GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).right);
    }
    private void IdleCheck()
    {
        float velo = Mathf.Abs(rb.velocity.x + rb.velocity.y);
        if (velo < 0.0001)
        {
            if (Time.time - 3 > time)
            {
                ani.SetBool("Time passed 5", true);
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
        
        if (actions.FindAction("Actions").triggered && !running)
        {
            running = true;
            if (!passed)
            {
                Player_Melee();
                passed = true;
            }
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

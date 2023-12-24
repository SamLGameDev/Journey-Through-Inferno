using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private float speed;
    private InputActionAsset actions;
    private float delay = -0.05f;
    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<PlayerInput>().actions;
    }
    public void Joystic_Movement()
    {
        // Gets the movement action and moves the player based on that times speed
       Vector2 movement = actions.FindAction("Movement").ReadValue<Vector2>();
       GetComponent<Rigidbody2D>().velocity = new Vector2(movement.x * speed, movement.y * speed);
    }
    private void Player_Melee()
    {
        float time = GetComponent<Different_Moves>().Melee(0.6f, 2, delay);
        delay = time;

    }
    // Update is called once per frame
    void Update()
    {
        
        if (actions.FindAction("Actions").IsPressed())
        {
              Player_Melee();
        }
        Joystic_Movement();
    }
}

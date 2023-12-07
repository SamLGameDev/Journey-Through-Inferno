using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Joystic_Movement()
    {
       InputActionAsset actions = GetComponent<PlayerInput>().actions;
       Vector2 movement = actions.FindAction("Movement").ReadValue<Vector2>();
       GetComponent<Rigidbody2D>().velocity = new Vector2(movement.x * speed, movement.y * speed);
    }
    // Update is called once per frame
    void Update()
    {
        Joystic_Movement();
    }
}

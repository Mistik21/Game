using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float Speed = 5f;
    private Rigidbody2D rigidbodyPlayer;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;
    

    void Start() 
    { 
        rigidbodyPlayer = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        var input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y = 1;
            if (Keyboard.current.sKey.isPressed) input.y = -1;
            if (Keyboard.current.aKey.isPressed)
            {
                input.x = -1;
                spriteRenderer.flipX = true;
            }
            if (Keyboard.current.dKey.isPressed)
            {

                input.x = 1;
                spriteRenderer.flipX = false;
            }
        }
        moveInput = input.normalized;
    }

    void FixedUpdate()
    {
        rigidbodyPlayer.linearVelocity = moveInput * Speed;
    }
}

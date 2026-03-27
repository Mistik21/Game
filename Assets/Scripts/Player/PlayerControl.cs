using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float Speed = 5f;
    public Animator animator;
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
            if (Keyboard.current.wKey.isPressed)
            {
                input.y = 1;
                animator.SetBool("stop",false);
            }
            if (Keyboard.current.sKey.isPressed)
            {
                input.y = -1;
                animator.SetBool("stop",false);
            }
            if (Keyboard.current.aKey.isPressed)
            {
                input.x = -1;
                spriteRenderer.flipX = true;
                animator.SetBool("stop",false);
            }
            if (Keyboard.current.dKey.isPressed)
            {

                input.x = 1;
                spriteRenderer.flipX = false;
                animator.SetBool("stop",false);
            }
        }
        moveInput = input.normalized;
    }

    void FixedUpdate()
    {
        rigidbodyPlayer.linearVelocity = moveInput * Speed;
        animator.SetBool("stop", moveInput == Vector2.zero);
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerScript : MonoBehaviour
{
    public float Money = 2f;
    public int Ammo = 200;
    public float Mana = 1000f;
    public float MaxMana = 1000f;
    public float Hp = 100f;
    public float MaxHp = 100f;
    public float Speed = 8.5f;
    public Animator Animation;
    private Rigidbody2D rigidbodyPlayer;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        Animation = GetComponent<Animator>();
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
                Animation.SetBool("stop", false);
            }

            if (Keyboard.current.sKey.isPressed)
            {
                input.y = -1;
                Animation.SetBool("stop", false);
            }

            if (Keyboard.current.aKey.isPressed)
            {
                input.x = -1;
                Flip(true);
                Animation.SetBool("stop", false);
            }

            if (Keyboard.current.dKey.isPressed)
            {
                input.x = 1;
                Flip(false);
                Animation.SetBool("stop", false);
            }
        }

        moveInput = input.normalized;
    }

    void FixedUpdate()
    {
        rigidbodyPlayer.linearVelocity = moveInput * Speed;
        Animation.SetBool("stop", moveInput == Vector2.zero);
    }

    void Flip(bool flip)
    {
        if (flip)
        {
            var scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }
    }
}
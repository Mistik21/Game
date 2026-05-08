using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    public bool isPaused = false;
    private GameObject Overlay;
    private Color originalColor;
    public bool end=false;
    public GameObject EndObject;


    void Start()
    {
        Animation = GetComponent<Animator>();
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (Hp <= 0)
        {
            if (!end)
            {
                Time.timeScale = 0f;
                end = true;
                isPaused = true;
                EndObject.SetActive(true);
                CreateDarkOverlay();
            }
        }

        var input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (!isPaused && !end)
            {
                if (GetMouseWorldPosition().x < transform.position.x)
                {
                    Flip(true);
                }
                else
                {
                    Flip(false);
                }
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
                    Animation.SetBool("stop", false);
                }

                if (Keyboard.current.dKey.isPressed)
                {
                    input.x = 1;
                    Animation.SetBool("stop", false);
                }

                if (Keyboard.current.escapeKey.wasReleasedThisFrame)
                {
                    PauseGame();
                }
            }
            else
            {
                if (Keyboard.current.escapeKey.wasReleasedThisFrame && isPaused && !end)
                {
                    if (MenuManager.Instance != null && MenuManager.Instance.isSettingsOpen)
                        MenuManager.Instance.CloseSettings();
                    else
                        ResumeGame();
                }
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
    private void OnTriggerEnter(Collider other)
    {
        // other — это объект-триггер, в который вошёл игрок
        if (other.CompareTag("Door"))
        {
            Destroy(other.gameObject);
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        CreateDarkOverlay();
        GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.SetActive(true);
        Debug.Log("Игра на паузе");
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject.SetActive(false);
        Destroy(Overlay);
    }
    void CreateDarkOverlay()
    {
        GameObject overlay = new GameObject("DarkOverlay");
        overlay.transform.SetParent(GameObject.Find("Canvas").transform);
        Image image = overlay.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0.75f); // черный, 50% прозрачности
    
        RectTransform rect = overlay.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        overlay.transform.SetSiblingIndex(1);
        Overlay= overlay;
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition =  Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
    
    public void FlashRed(float duration = 0.1f)
    {
        spriteRenderer.color = Color.red;
        CancelInvoke("ResetColor");
        Invoke("ResetColor", duration);
    }

    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }
}
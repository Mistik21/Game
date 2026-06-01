using System;
using System.Collections.Generic;
using RiflePlayer;
using UnityEngine;
using UnityEngine.InputSystem;

public class inventory : MonoBehaviour
{
    public int indexInventary = 0;
    public string targetTag = "PlayerWeapon"; // Тег искомых объектов
    public float detectionRange = 1.3f;
    public GameObject[] Inventory = new GameObject[2];
    private List<GameObject> nearbyEnemies = new List<GameObject>();


    void Start()
    {
        for (var i = 0; i < Inventory.Length; i++)
        {
            if (i == indexInventary)
            {
                ActiveInventory();
            }
            else if (Inventory[i] != null)
            {
                Inventory[i].SetActive(false);
            }
        }
    }

    public int IndexInventory()
    {
        return indexInventary;
    }

    void Update()
    {
        if (Time.timeScale != 0f)
        {
            var scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0)
            {
                DeactivateInventory();
                indexInventary = (indexInventary + 1) % 2;
                ActiveInventory();
            }
            else if (scroll < 0)
            {
                DeactivateInventory();
                indexInventary = Math.Abs(indexInventary - 1) % 2;
                ActiveInventory();
            }

            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                if (Inventory[indexInventary])
                {
                    Inventory[indexInventary].transform.parent = null;
                    try
                    {
                        Inventory[indexInventary].GetComponent<DestrouTransfer>().enabled = true;
                    }
                    catch (Exception e)
                    {
                        var t = 0;
                    }
                    Inventory[indexInventary] = null;
                }
            }

            PickUpWeapon();
        }
    }

    void PickUpWeapon()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(targetTag);

        nearbyEnemies.Clear();

        foreach (GameObject obj in allObjects)
        {
            if (Inventory[indexInventary] && obj == Inventory[indexInventary])
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, obj.transform.position);

            if (distance <= detectionRange)
            {
                nearbyEnemies.Add(obj);
            }
        }

        if (nearbyEnemies.Count > 0)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (nearbyEnemies[0].GetComponent<BaseWeapon>() && nearbyEnemies[0].GetComponent<BaseWeapon>().sale)
                {
                    if (nearbyEnemies[0].GetComponent<BaseWeapon>().price <= GetComponent<PlayerScript>().Money)
                    {
                        SoundEffectsManager.Instance?.PlayItemPurchase();
                        GetComponent<PlayerScript>().Money -= nearbyEnemies[0].GetComponent<BaseWeapon>().price;
                        nearbyEnemies[0].GetComponent<BaseWeapon>().sale = false;
                    }
                    else
                    {
                        return;
                    }
                }
                
                SoundEffectsManager.Instance?.PlayWeaponPickup();

                if (Inventory[indexInventary])
                {
                    Inventory[indexInventary].transform.parent = null;
                    try
                    {
                        Inventory[indexInventary].GetComponent<DestrouTransfer>().enabled = true;
                    }
                    catch (Exception e)
                    {
                        var t = 0;
                    }
                }

                Inventory[indexInventary] = nearbyEnemies[0];
                Inventory[indexInventary].transform.SetParent(transform);
                var scale = Inventory[indexInventary].transform.localScale;
                scale.x = Math.Abs(scale.x);
                Inventory[indexInventary].transform.localScale = scale;
                Inventory[indexInventary].transform.localPosition = new Vector3(0.34f, -0.2f, 0);
            }
        }
    }


    void ActiveInventory()
    {
        if (Inventory[indexInventary])
        {
            Inventory[indexInventary].SetActive(true);
        }
    }

    void DeactivateInventory()
    {
        if (Inventory[indexInventary])
        {
            Inventory[indexInventary].SetActive(false);
        }
    }
}
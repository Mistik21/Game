using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inventory : MonoBehaviour
{
    public string targetTag = "PlayerWeapon";   // Тег искомых объектов
    public float detectionRange = 1.3f;
    public GameObject[] Inventory = new GameObject[2];
    private int indexInventary = 0;
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


    void Update()
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
            indexInventary = Math.Abs(indexInventary -1) % 2;
            ActiveInventory();
        }

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            if (Inventory[indexInventary])
            {
                Inventory[indexInventary].transform.parent = null;
                Inventory[indexInventary] = null;
            }
        }
        if(!Inventory[indexInventary])
        {
            PickUpWeapon();
        }
    }
    void PickUpWeapon()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(targetTag);

        nearbyEnemies.Clear();

        foreach (GameObject obj in allObjects)
        {
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
                Inventory[indexInventary]=nearbyEnemies[0];
                Inventory[indexInventary].transform.SetParent(transform);
                var scale = Inventory[indexInventary].transform.localScale;
                scale.x = Math.Abs(scale.x);
                Inventory[indexInventary].transform.localScale = scale;
                Inventory[indexInventary].transform.localPosition  = new Vector3(0.34f, -0.2f, 0);
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
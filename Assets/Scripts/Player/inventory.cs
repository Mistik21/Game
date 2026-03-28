using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class inventory : MonoBehaviour
{
    public GameObject[] Inventory = new GameObject[2];
    private int indexInventary = 0;

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
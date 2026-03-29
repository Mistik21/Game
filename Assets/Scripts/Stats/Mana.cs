using System;
using UnityEngine;
using UnityEngine.UI;


public class Mana : MonoBehaviour
{
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = Player.GetComponent<PlayerScript>().Mana/Player.GetComponent<PlayerScript>().MaxMana;
    }
}

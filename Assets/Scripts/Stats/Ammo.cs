using UnityEngine;
using TMPro;

public class Ammo : MonoBehaviour
{
    public GameObject Player;
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = Player.GetComponent<PlayerScript>().Ammo.ToString();
    }
}
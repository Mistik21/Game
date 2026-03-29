using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    public GameObject Player;
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = Player.GetComponent<PlayerScript>().Money.ToString();
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGameSpavn : MonoBehaviour
{
    private bool isPlayerInside = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
    
    void Update()
    {
        if (isPlayerInside && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            var user = GameObject.FindWithTag("Player");
            user.GetComponent<TransferPlayer>().enabled=true;
            foreach (Transform child in user.transform)
            {
                if (child.CompareTag("Light"))
                {
                    child.gameObject.SetActive(true);
                }
            }
            SceneManager.LoadScene("Level1");
        }
    }
}
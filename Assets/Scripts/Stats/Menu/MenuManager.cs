using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitToMenu()
    {
        MusicManager.Instance.TurnOffMusic();
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ContinueGame()
    {
        Debug.Log("Continue Game");
        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().ResumeGame();
    }
}

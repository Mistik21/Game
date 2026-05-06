using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MenuManager Instance;
    public bool isSettingsOpen = false;
    public GameObject SettingsPanel;

    void Start()
    {
        Instance = this;
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

    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        isSettingsOpen = true;
    }

    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
        isSettingsOpen = false;
    }
}

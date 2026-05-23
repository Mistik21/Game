using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MenuManager Instance;
    public bool isSettingsOpen = false;
    public GameObject SettingsPanel;
    public GameObject Info;

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
        GameObject target = GameObject.FindWithTag("Load").GetComponentsInChildren<Transform>(true)[1].gameObject;
        target.SetActive(true);
        MusicManager.Instance.TurnOffMusic();
        Time.timeScale = 1f;
        try
        {
            Transfer[] allEnemies = Object.FindObjectsByType<Transfer>(FindObjectsSortMode.None);
            TransferPlayer Enemies = Object.FindObjectsByType<TransferPlayer>(FindObjectsSortMode.None)[0];
            SceneManager.MoveGameObjectToScene(Enemies.gameObject, SceneManager.GetActiveScene());
            foreach (Transfer enemy in allEnemies)
            {
                enemy.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(enemy.gameObject, SceneManager.GetActiveScene());
            }
        }
        catch (Exception e)
        {
            var a = 0;
        }
        SceneManager.LoadScene("MainMenu");
    }
    public void ContinueGame()
    {
        Debug.Log("Continue Game");
        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().ResumeGame();
    }
    public void InformationGame()
    {
        Info.SetActive(true);
    }
    public void CloseInformationGame()
    {
        Info.SetActive(false);
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

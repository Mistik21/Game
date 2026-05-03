using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGameSpavn : MonoBehaviour
{
    private bool isPlayerInside = false;
    private List<string> scenePaths = new List<string>();

    void Start()
    {
        string folderPath = "Assets/Scenes/Levels"; // Укажите вашу папку
        
        // Найти все файлы .unity в папке
        string[] guids = AssetDatabase.FindAssets("t:Scene", new[] { folderPath });
        
        scenePaths = new List<string>();
        foreach (string guid in guids)
        {
            scenePaths.Add(AssetDatabase.GUIDToAssetPath(guid));
        } 
    }
    
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
            var random=new System.Random().Next(0,scenePaths.Count);
            SceneManager.LoadScene(scenePaths[random]);
        }
    }
}
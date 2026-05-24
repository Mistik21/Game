using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartGameSpavn : MonoBehaviour
{
    private bool isPlayerInside = false;
    private List<string> scenePaths = new List<string>();

    void Start()
    {
        scenePaths = new List<string>();

        // ЛЕГАЛЬНЫЙ СПОСОБ ДЛЯ БИЛДА: Получаем все сцены, которые ты добавил в окно Build Settings / Build Profiles
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            
            // Фильтруем сцены, чтобы брать только из папки Levels (как у тебя и было задумано)
            if (scenePath.Contains("Assets/Scenes/Levels"))
            {
                scenePaths.Add(scenePath);
            }
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
            if (SceneManager.GetActiveScene().name == "Training")
            {
                ActivateEndWinUI();
            }
            else if (GameObject.FindWithTag("Player").GetComponent<PlayerScript>().Levels.Count < 1)
            {
                if (scenePaths.Count == 0)
                {
                    Debug.LogError("Список сцен пуст! Проверь, добавил ли ты сцены уровней в окно Build Profiles (Build Settings)");
                    return;
                }

                MusicManager.Instance?.StartMusic();
                
                GameObject loadTag = GameObject.FindWithTag("Load");
                if (loadTag != null)
                {
                    GameObject target = loadTag.GetComponentsInChildren<Transform>(true)[1].gameObject;
                    target.SetActive(true);
                }

                var user = GameObject.FindWithTag("Player");
                var playerScript = user.GetComponent<PlayerScript>();
                
                user.GetComponent<TransferPlayer>().enabled = true;
                foreach (Transform child in user.transform)
                {
                    if (child.CompareTag("Light"))
                    {
                        child.gameObject.SetActive(true);
                    }
                }

                // Выбираем случайную сцену из тех, что добавлены в билд
                var random = new System.Random().Next(0, scenePaths.Count);
                int attempts = 0;

                while (playerScript.Levels.Contains(scenePaths[random]) && attempts < 100)
                {
                    random = new System.Random().Next(0, scenePaths.Count);
                    attempts++;
                }

                playerScript.Levels.Add(scenePaths[random]);
                SceneManager.LoadScene(scenePaths[random]);
            }
            else
            {
                ActivateEndWinUI();
            }
        }
    }

    // Вынес повторяющийся поиск UI-окна победы в отдельный чистый метод
    private void ActivateEndWinUI()
    {
        // Находим все объекты, включая выключенные на сцене
        
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // Вместо EditorUtility.IsPersistent проверяем, принадлежит ли объект активной сцене. 
            // Если scene.name равен null, значит это префаб в ассетах, а не объект на сцене.
            if (obj.CompareTag("EndWin") && obj.scene.name != null)
            {
                MusicManager.Instance?.TurnOffMusic();
                obj.SetActive(true);
                break; // Нашли — включаем и выходим
            }
        }
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
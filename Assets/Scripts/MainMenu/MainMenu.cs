using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для кнопки "Начать игру"
    public Texture2D myCustomCursor;
    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    public void StartGame()
    {
        Cursor.SetCursor(myCustomCursor, new Vector2(32,32), CursorMode.Auto);
        GameObject target = GameObject.FindWithTag("Load").GetComponentsInChildren<Transform>(true)[1].gameObject;
        target.SetActive(true);
        SceneManager.LoadScene("Spawn"); // Укажите имя вашей игровой сцены
    }
    public void TrainingGame()
    {
        Cursor.SetCursor(myCustomCursor, new Vector2(32,32), CursorMode.Auto);
        GameObject target = GameObject.FindWithTag("Load").GetComponentsInChildren<Transform>(true)[1].gameObject;
        target.SetActive(true);
        SceneManager.LoadScene("Training"); // Укажите имя вашей игровой сцены
    }
    public void ExitGame()
    {
        
#if UNITY_EDITOR
        // Если в редакторе Unity - останавливаем игру
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Если в собранной игре - закрываем приложение
            Application.Quit();
#endif
    }
}
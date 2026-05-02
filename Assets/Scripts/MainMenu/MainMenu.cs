using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для кнопки "Начать игру"
    public void StartGame()
    {
        SceneManager.LoadScene("Spawn"); // Укажите имя вашей игровой сцены
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
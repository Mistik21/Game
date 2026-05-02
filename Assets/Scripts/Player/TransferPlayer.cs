using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class TransferPlayer : MonoBehaviour
{
    private Vector2 targetPosition = new Vector2(0, 0);
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Переносится ВЕСЬ объект
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Устанавливаем позицию объекта после загрузки сцены
        transform.position = targetPosition;
        enabled = false;
    }
}
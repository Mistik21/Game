using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuManager : MonoBehaviour
{
    public List<GameObject> dels;
    public GameObject canvas;
    public GameObject player;
    
    public void ExitToSpavn()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.ResetMusic();
            
        StartCoroutine(DestroyAllAndLoad());
    }
    
    IEnumerator DestroyAllAndLoad()
    {
        Time.timeScale = 1f;
        
        // Создаём временный список, чтобы избежать проблем с изменением исходного
        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in dels)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        canvas.SetActive(false);
        player.SetActive(false);
        if (canvas) objectsToDestroy.Add(canvas);
        if (player) objectsToDestroy.Add(player);
        objectsToDestroy.AddRange(dels);
        
        // Уничтожаем каждый объект отдельно
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                // Отключаем объект перед удалением
                obj.SetActive(false);
                Destroy(obj);
            }
            // Даём Unity время на обработку
            yield return null;
        }
        
        dels.Clear();
        
        // Загружаем новую сцену
        SceneManager.LoadScene("Spawn");
        Destroy(gameObject);
    }
}
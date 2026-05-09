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
        StartCoroutine(DestroyAllAndLoad());
        GameObject target = GameObject.FindWithTag("Load").GetComponentsInChildren<Transform>(true)[1].gameObject;
        target.SetActive(true);
    }
    
    IEnumerator DestroyAllAndLoad()
    {
        Time.timeScale = 1f;
        
        // Создаём временный список, чтобы избежать проблем с изменением исходного
        List<GameObject> objectsToDestroy = new List<GameObject>();
        canvas.SetActive(false);
        player.SetActive(false);
        if (canvas) objectsToDestroy.Add(canvas);
        if (player) objectsToDestroy.Add(player);
        objectsToDestroy.AddRange(dels.GetRange(0, 2));
        
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
        
        SceneManager.LoadScene("Spawn");
        foreach (GameObject obj in dels)
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
        Destroy(gameObject);
    }
}
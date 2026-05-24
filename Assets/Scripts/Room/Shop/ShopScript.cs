using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random; // Оставляем твой системный рандом

public class ShopScript : MonoBehaviour
{
    public GameObject[] spavns = new GameObject[4];
    public List<GameObject> PrefabsSale;

    void Start()
    {
        // ВАЖНО: Путь для Resources.LoadAll пишется ОТНОСИТЕЛЬНО папки Resources.
        // Так как твоя папка лежит в Assets/Resources/Prefabs/SalePrefabObject,
        // то мы пишем только то, что идет ПОСЛЕ слова Resources/
        string folderPath = "Prefabs/SalePrefabObject"; 
        
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(folderPath);
        
        PrefabsSale.Clear();
        PrefabsSale.AddRange(loadedPrefabs);

        // ЗАЩИТА: Если пути перепутаны или папка пуста, игра не вылетит, а вежливо предупредит
        if (PrefabsSale.Count == 0)
        {
            Debug.LogError($"[ShopScript] Не удалось найти префабы товаров по пути: Assets/Resources/{folderPath}. Проверь имя папки!");
            return;
        }

        var random = new Random();
        foreach (var spavn in spavns)
        {
            if (spavn == null) continue;

            var index = random.Next(0, PrefabsSale.Count);
            
            // Стандартный Instantiate вместо PrefabUtility.InstantiatePrefab
            GameObject instance = Instantiate(PrefabsSale[index]);
            
            instance.transform.position = spavn.transform.position;
            instance.transform.SetParent(transform);
        }
    }

    void Update()
    {
        
    }
}
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using Random = System.Random;

public class ShopScript : MonoBehaviour
{
    public GameObject[] spavns = new GameObject[4];
    public List<GameObject> PrefabsSale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string folderPath = "Assets/Prefabs/SalePrefabObject"; 
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        foreach (string guid in guids)
        {
            // Преобразуем GUID в путь к файлу.
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // Загружаем префаб по пути.
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            PrefabsSale.Add(prefab);
        }
        var random = new Random();
        foreach (var spavn in spavns)
        {
            var index = random.Next(0, PrefabsSale.Count - 1);
            var instance = PrefabUtility.InstantiatePrefab(PrefabsSale[index]) as GameObject;
            instance.transform.position = spavn.transform.position;
            instance.transform.SetParent(transform);
        }
    }
    
    void Update()
    {
        
    }
}

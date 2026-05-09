using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Randoms = System.Random;

public class RoomScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> NPCs;
    public List<GameObject> Doors;
    public GameObject Controler;
    public GameObject Area;
    public int MaxNPC=4;
    public List<GameObject> PrefabsNPC;
    [Header("Область поиска")]
    [SerializeField] private Vector2 spawnAreaCenter ;
    [SerializeField] private float spawnRadius = 12f;      // Радиус области
    
    [Header("Настройки NavMesh")]
    [SerializeField] private float sampleRadius = 2f;      // Радиус поиска NavMesh
    [SerializeField] private int maxAttempts = 30; 
    void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            // Пропускаем сам родительский объект
            if (child == transform) continue;
            
            // Если у дочернего объекта есть тег "Door", добавляем его в список
            if (child.CompareTag("Door"))
            {
                Doors.Add(child.gameObject);
            }
        }
        spawnAreaCenter=Area.transform.position;
        string folderPath = "Assets/Prefabs/NPCPrefabs"; 
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        foreach (string guid in guids)
        {
            // Преобразуем GUID в путь к файлу.
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // Загружаем префаб по пути.
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            PrefabsNPC.Add(prefab);
        }
        SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (NPCs.All(obj => !obj))
        {
            MusicManager.Instance.ExitCombat();

            foreach(var door in Doors)
            {
                Destroy(door);
            }
            Destroy(Controler);
            Destroy(Area);
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            var random = new Randoms();
            player.Mana += random.Next(0,(int)Math.Min((player.MaxMana-player.Mana)+1,200)+1);
            player.Money += random.Next(1,6);
            enabled = false;
        }
    }
    public void SpawnObjects()
    {
        foreach (var NPC in PrefabsNPC)
        {
            if (NPCs.Count >= MaxNPC)
            {
                break;
            }
            for (int i = 1; i < Random.Range(1, MaxNPC); i++)
            {
                Vector3 spawnPoint = GetRandomPointOnNavMesh();
                if (spawnPoint != Vector3.zero)
                {
                    GameObject newNPC = Instantiate(NPC, spawnPoint, Quaternion.identity);
                    NPCs.Add(newNPC);
                }
            }
        }
        if (NPCs.Count == 0)
        {
            SpawnObjects();
        }
    }
    private Vector3 GetRandomPointOnNavMesh()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            // Случайная точка в круге
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPoint = new Vector3(
                spawnAreaCenter.x + randomCircle.x,
                spawnAreaCenter.y + randomCircle.y,
                0f
            );
            
            // Ищем ближайшую точку на NavMesh [citation:2][citation:5]
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, sampleRadius, NavMesh.AllAreas))
            {
                return hit.position;  // Точка гарантированно на NavMesh!
            }
        }
        
        return Vector3.zero;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnAreaCenter, spawnRadius);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnAreaCenter, sampleRadius);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Randoms = System.Random; // Оставляем твой системный рандом для подсчета маны/монет

public class RoomScript : MonoBehaviour
{
    public List<GameObject> NPCs;
    public List<GameObject> Doors;
    public GameObject Controler;
    public GameObject Area;
    public int MaxNPC = 4;
    public List<GameObject> PrefabsNPC;

    [Header("Область поиска")]
    [SerializeField] private Vector2 spawnAreaCenter;
    [SerializeField] private float spawnRadius = 12f;      // Радиус области
    
    [Header("Настройки NavMesh")]
    [SerializeField] private float sampleRadius = 2f;      // Радиус поиска NavMesh
    [SerializeField] private int maxAttempts = 30; 

    void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue;
            
            if (child.CompareTag("Door"))
            {
                Doors.Add(child.gameObject);
            }
        }

        // Принудительно зануляем Z для 2D, чтобы NavMesh не терялся
        spawnAreaCenter = new Vector2(Area.transform.position.x, Area.transform.position.y);

        // Загрузка префабов через ресурсы (легально для билда)
        string folderPath = "Prefabs/NPCPrefabs"; 
        GameObject[] npcPrefabs = Resources.LoadAll<GameObject>(folderPath);
        
        PrefabsNPC.Clear();
        PrefabsNPC.AddRange(npcPrefabs);

        SpawnObjects();
    }

    void Update()
    {
        if (NPCs.All(obj => !obj))
        {
            // Сразу выключаем скрипт, чтобы код ниже не вызвался повторно на следующем кадре
            enabled = false;

            MusicManager.Instance.ExitCombat();
            SoundEffectsManager.Instance.PlayRoomClear();
            
            foreach(var door in Doors)
            {
                if (door != null) Destroy(door);
            }
            if (Controler != null) Destroy(Controler);
            if (Area != null) Destroy(Area);

            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                var player = playerObj.GetComponent<PlayerScript>();
                var random = new Randoms();
                player.Mana += random.Next(0, (int)Math.Min((player.MaxMana - player.Mana) + 1, 200) + 1);
                player.Money += random.Next(1, 6);
            }
        }
    }

    public void SpawnObjects()
    {
        if (PrefabsNPC.Count == 0)
        {
            Debug.LogError("Нет префабов для спавна в PrefabsNPC! Проверь папку Assets/Resources/Prefabs/NPCPrefabs");
            return;
        }

        int attemptsToSpawnTotal = 0; // Защита от бесконечного цикла

        // Спавним случайных врагов, пока не забьем комнату до MaxNPC
        while (NPCs.Count < MaxNPC && attemptsToSpawnTotal < 100)
        {
            attemptsToSpawnTotal++;

            Vector3 spawnPoint = GetRandomPointOnNavMesh();
            if (spawnPoint != Vector3.zero)
            {
                // Выбираем случайный префаб из загруженных
                GameObject randomPrefab = PrefabsNPC[Random.Range(0, PrefabsNPC.Count)];
                GameObject newNPC = Instantiate(randomPrefab, spawnPoint, Quaternion.identity);
                NPCs.Add(newNPC);
            }
        }
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPoint = new Vector3(
                spawnAreaCenter.x + randomCircle.x,
                spawnAreaCenter.y + randomCircle.y,
                0f
            );
            
            NavMeshHit hit;
            // Плавное увеличение радиуса поиска с каждой попыткой (фикс для 2D)
            float currentRadius = sampleRadius + (i * 0.2f);

            if (NavMesh.SamplePosition(randomPoint, out hit, currentRadius, NavMesh.AllAreas))
            {
                return hit.position;
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
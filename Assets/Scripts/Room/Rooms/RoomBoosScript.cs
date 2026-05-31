using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Randoms = System.Random;

public class RoomBoosScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> NPCs;
    public List<GameObject> Doors;
    public GameObject Controler;
    public GameObject Area;
    public GameObject PrefabNPC;
    public GameObject TPNextLevel;

    [Header("Область поиска")] [SerializeField]
    private Vector2 spawnAreaCenter;

    [SerializeField] private float spawnRadius = 12f; // Радиус области

    [Header("Настройки NavMesh")] [SerializeField]
    private float sampleRadius = 2f; // Радиус поиска NavMesh

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

        spawnAreaCenter = Area.transform.position;
        SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (NPCs.All(obj => !obj))
        {
            MusicManager.Instance.ExitCombat();
            SoundEffectsManager.Instance.PlayRoomClear();

            foreach (var door in Doors)
            {
                Destroy(door);
            }

            Destroy(Controler);
            Destroy(Area);
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            var random = new Randoms();
            player.Mana += random.Next(0, (int)Math.Min((player.MaxMana - player.Mana), 200) + 1);
            player.Money += random.Next(15, 30);
            TPNextLevel.SetActive(true);
            enabled = false;
        }
    }

    public void SpawnObjects()
    {
        Vector3 spawnPoint = GetRandomPointOnNavMesh();
        Debug.Log(spawnPoint);
        if (spawnPoint != Vector3.zero)
        {
            GameObject newNPC = Instantiate(PrefabNPC, spawnPoint, Quaternion.identity);
            newNPC.GetComponent<SpriteRenderer>().flipX = true;
            Debug.Log(newNPC);
            NPCs.Add(newNPC);
            
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
                return hit.position; // Точка гарантированно на NavMesh!
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
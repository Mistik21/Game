using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RoomScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> NPCs;
    public GameObject Door;
    public GameObject Controler;
    public GameObject Area;
    public GameObject NPCPrefab;
    private int MaxNPC=5;
    [Header("Область поиска")]
    [SerializeField] private Vector2 spawnAreaCenter ;
    [SerializeField] private float spawnRadius = 12f;      // Радиус области
    
    [Header("Настройки NavMesh")]
    [SerializeField] private float sampleRadius = 2f;      // Радиус поиска NavMesh
    [SerializeField] private int maxAttempts = 30; 
    void Start()
    {
        spawnAreaCenter=Area.transform.position;
        SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (NPCs.All(obj => !obj))
        {
            Destroy(Door);
            Destroy(Controler);
            Destroy(Area);
            enabled = false;
        }
    }
    public void SpawnObjects()
    {
        for (int i = 1; i < Random.Range(2, MaxNPC); i++)
        {
            Vector3 spawnPoint = GetRandomPointOnNavMesh();
            if (spawnPoint != Vector3.zero)
            {
                GameObject newNPC = Instantiate(NPCPrefab, spawnPoint, Quaternion.identity);
                NPCs.Add(newNPC);
            }
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

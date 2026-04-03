using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public float Hp = 100f;
    public float MinDistanceToPlayer = 3f;
    private GameObject player;
    private Transform playerTransform;
    private NavMeshAgent navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            enabled = false;
            Destroy(gameObject,0.1f);
        }

        navMeshAgent.SetDestination(player.transform.position);
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > MinDistanceToPlayer ||
            IsWallBetween(player.transform, transform, LayerMask.GetMask("Wall", "Obstacle")))
        {
            // Игрок далеко — идём к нему
            navMeshAgent.SetDestination(player.transform.position);
            navMeshAgent.stoppingDistance = MinDistanceToPlayer; // Остановится на дистанции
        }
        else
        {
            // Игрок слишком близко — стоим на месте
            navMeshAgent.SetDestination(transform.position);
        }

        DirectionOfTheModel();
    }

    bool IsWallBetween(Transform player, Transform npc, LayerMask wallLayer)
    {
        Vector2 start = player.position;
        Vector2 end = npc.position;

        // Делаем Linecast от игрока к NPC
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);


        foreach (RaycastHit2D hit in hits)
        {
            // Пропускаем самого игрока и NPC
            if (hit.transform == player || hit.transform == npc)
                continue;

            // Если это стена - возвращаем true
            if (hit.collider.CompareTag("Wall"))
                return true;
        }

        return false;
    }

    void DirectionOfTheModel()
    {
        if (player.transform.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip(true);
        }
        else if (player.transform.position.x > transform.position.x && transform.localScale.x < 0)
        {
            Flip(false);
        }
    }

    void Flip(bool flip)
    {
        if (flip)
        {
            var scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
        else
        {
            var scale = transform.localScale;
            scale.x = Math.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
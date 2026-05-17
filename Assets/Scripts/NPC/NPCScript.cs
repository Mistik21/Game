using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public float Hp = 100f;
    public float MinDistanceToPlayer = 3f;
    public LayerMask wallLayer; // Вынеси в инспектор

    private GameObject player;
    private NavMeshAgent navMeshAgent;
    private Animator Animation;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private float pathUpdateDeadline; // Для оптимизации поиска пути

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        // Для 2D
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        
        navMeshAgent.stoppingDistance = MinDistanceToPlayer; // Устанавливаем один раз

        Animation = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        
        wallLayer = LayerMask.GetMask("Wall", "Obstacle");
    }

    void Update()
    {
        // Вместо создания нового Quaternion, лучше заблокировать вращение в Rigidbody или NavMesh
        transform.rotation = Quaternion.identity;

        if (Hp <= 0)
        {
            Destroy(gameObject, 0.1f);
            return;
        }

        if (player != null)
        {
            // Оптимизация: обновляем путь 5 раз в секунду, а не каждый кадр
            if (Time.time > pathUpdateDeadline)
            {
                pathUpdateDeadline = Time.time + 0.2f;
                
                bool wallBetween = IsWallBetween();
                float distance = Vector2.Distance(transform.position, player.transform.position);

                // Если есть стена — идем к игроку игнорируя дистанцию остановки (чтобы зайти в комнату)
                if (wallBetween)
                {
                    navMeshAgent.stoppingDistance = 0; 
                    navMeshAgent.SetDestination(player.transform.position);
                }
                else
                {
                    navMeshAgent.stoppingDistance = MinDistanceToPlayer;
                    navMeshAgent.SetDestination(player.transform.position);
                }
            }

            Animation.SetBool("stop", !IsMoving());
            DirectionOfTheModel();
        }
    }

    public bool IsWallBetween()
    {
        Vector2 start = transform.position; // Лучше начинать от NPC к Игроку
        Vector2 end = player.transform.position;
        float distance = Vector2.Distance(start, end);

        // Используем Raycast для проверки стены
        RaycastHit2D hit = Physics2D.Raycast(start, end - start, distance, wallLayer);

        if (hit.collider != null)
        {
            return true; // На пути есть слой Wall или Obstacle
        }

        return false;
    }
    void DirectionOfTheModel()
    {
        // Рассчитываем разницу позиций между игроком и NPC
        float directionToPlayer = player.transform.position.x - transform.position.x;
    
        // Если игрок слева (отрицательное значение), передаем true для флипа
        if (directionToPlayer < -0.1f)
        {
            Flip(true);
        }
        // Если игрок справа (положительное значение), передаем false
        else if (directionToPlayer > 0.1f)
        {
            Flip(false);
        }
    }

    void Flip(bool shouldFlip)
    {
        Vector3 scale = transform.localScale;
        // Устанавливаем масштаб по X: отрицательный если shouldFlip, иначе положительный
        scale.x = shouldFlip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private bool IsMoving()
    {
        return navMeshAgent.velocity.magnitude > 0.1f && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
    }

    public void FlashRed(float duration = 0.1f)
    {
        spriteRenderer.color = Color.red;
        CancelInvoke(nameof(ResetColor));
        Invoke(nameof(ResetColor), duration);
    }

    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }

    void OnDrawGizmos() {
        if (navMeshAgent != null && navMeshAgent.hasPath) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, navMeshAgent.destination);
        }
    }
}
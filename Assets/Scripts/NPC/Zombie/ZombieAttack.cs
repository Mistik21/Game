using System.Collections;
using UnityEngine;

namespace GunNPC
{
    public class ZombieAttack : MonoBehaviour
    {
        [Header("Настройки атаки")]
        [SerializeField] private float attackRange = 1.8f;      // Дистанция атаки
        [SerializeField] private float attackDamage = 20f;      // Урон за удар
        [SerializeField] private float attackCooldown = 1f;     // Задержка между ударами
        
        [Header("Ссылки")]
        [SerializeField] private string playerTag = "Player";
        
        private NPCScript npcScript;
        private GameObject player;
        private Transform playerTransform;
        
        private float lastAttackTime;
        private bool isAttacking;
        private UnityEngine.AI.NavMeshAgent navMeshAgent;
        
        void Start()
        {
            npcScript = GetComponent<NPCScript>();
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            
            player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                playerTransform = player.transform;
            }
            
            lastAttackTime = -attackCooldown;
        }
        
        void Update()
        {
            if (npcScript != null && npcScript.Hp <= 0) return;
            if (playerTransform == null) return;
            
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            
            // Проверяем, находится ли игрок в зоне атаки
            if (distanceToPlayer <= attackRange)
            {
                // Добавляем проверку на стены, используя существующий метод из NPCScript
                bool isWallBetween = false;
                if (npcScript != null)
                {
                    isWallBetween = npcScript.IsWallBetween(LayerMask.GetMask("Wall", "Obstacle"));
                }
                
                // Атакуем только если нет стены между NPC и игроком
                if (!isWallBetween)
                {
                    TryAttack();
                }
            }
        }
        
        private void TryAttack()
        {
            // Проверяем, прошло ли достаточно времени для новой атаки
            if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                Attack();
            }
        }
        
        private void Attack()
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack());
        }
        
        private IEnumerator PerformAttack()
        {
            isAttacking = true;
            
            // Приостанавливаем движение во время атаки
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
            }
            
            // Небольшая задержка перед нанесением урона (эффект замаха)
            yield return new WaitForSeconds(0.2f);
            
            // Наносим урон игроку
            DealDamageToPlayer();
            
            // Эффект вспышки для зомби (опционально)
            if (npcScript != null)
            {
                npcScript.FlashRed(0.1f);
            }
            
            // Небольшая задержка после удара
            yield return new WaitForSeconds(0.1f);
            
            // Возобновляем движение
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = false;
            }
            
            isAttacking = false;
        }
        
        private void DealDamageToPlayer()
        {
            if (player == null) 
            {
                player = GameObject.FindGameObjectWithTag(playerTag);
                if (player == null) return;
            }
            
            // Получаем компонент PlayerScript и наносим урон
            PlayerScript playerScript = player.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.Hp -= attackDamage;
                playerScript.FlashRed(0.1f);
                Debug.Log($"Zombie attacked! Player HP: {playerScript.Hp}");
            }
            else
            {
                Debug.LogWarning($"PlayerScript component not found on {player.name}!");
            }
        }
        
        // Метод для проверки, может ли NPC атаковать в данный момент
        public bool CanAttack()
        {
            if (playerTransform == null) return false;
            
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            return distanceToPlayer <= attackRange && !isAttacking && (Time.time >= lastAttackTime + attackCooldown);
        }
        
        // Визуализация зоны атаки в редакторе
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace GunNPC
{
    public class ZombieAttack : MonoBehaviour
    {
        [Header("Настройки атаки")]
        [SerializeField] private float attackRange = 1.8f;
        [SerializeField] private float attackDamage = 20f;
        [SerializeField] private float attackCooldown = 1f;
        
        [Header("Ссылки")]
        [SerializeField] private string playerTag = "Player";
        
        private NPCScript npcScript;
        private GameObject player;
        private Transform playerTransform;
        private Animator anim; // Добавляем ссылку на аниматор
        
        private float lastAttackTime;
        private bool isAttacking;
        private UnityEngine.AI.NavMeshAgent navMeshAgent;
        
        void Start()
        {
            npcScript = GetComponent<NPCScript>();
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<Animator>(); // Инициализируем аниматор
            
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
            
            if (distanceToPlayer <= attackRange)
            {
                bool isWallBetween = false;
                if (npcScript != null)
                {
                    isWallBetween = npcScript.IsWallBetween();
                }
                
                if (!isWallBetween)
                {
                    TryAttack();
                }
            }
        }
        
        private void TryAttack()
        {
            if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                Attack();
            }
        }
        
        private void Attack()
        {
            lastAttackTime = Time.time;

            // Запускаем анимацию прямо здесь перед началом корутины
            if (anim != null)
            {
                // Убедись, что название клипа в Animator именно "Attack"
                anim.Play("Attack", 0, 0f);
            }

            StartCoroutine(PerformAttack());
        }
        
        private IEnumerator PerformAttack()
        {
            isAttacking = true;
            
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
            }
            
            // Время "замаха" должно совпадать с моментом удара на твоей анимации
            yield return new WaitForSeconds(0.4f);
            
            DealDamageToPlayer();
            
            if (npcScript != null)
            {
                npcScript.FlashRed(0.1f);
            }
            
            yield return new WaitForSeconds(0.1f);
            
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

        public bool CanAttack()
        {
            if (playerTransform == null) return false;
            
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            return distanceToPlayer <= attackRange && !isAttacking && (Time.time >= lastAttackTime + attackCooldown);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
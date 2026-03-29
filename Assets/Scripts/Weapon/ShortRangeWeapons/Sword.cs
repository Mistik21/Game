using UnityEngine;
using UnityEngine.InputSystem;

namespace Sword
{
    public class Sword : MonoBehaviour
    {
        [Header("Настройки атаки")] public int damage = 25;
        public float attackRange = 10.5f;
        public float attackRate = 5f;
        private float nextAttackTime = 0f;

        [Header("Точка удара")] public Transform attackPoint;
        public LayerMask enemyLayers;

        void Update()
        {
            Transform parentTransform = transform.parent;
            if (parentTransform)
            {
                // Атака по левой кнопке мыши
                if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + attackRate;
                }
            }
        }

        void Attack()
        {
            // Поиск врагов в радиусе атаки
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            // Нанесение урона
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("sese");
                enemy.GetComponent<NPCScript>().Hp-=damage;
            }

            if (hitEnemies.Length == 0)
            {
                Debug.Log("Промах!");
            }
        }

        // Визуализация радиуса атаки в редакторе
        void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
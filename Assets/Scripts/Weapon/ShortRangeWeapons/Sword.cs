using UnityEngine;
using UnityEngine.InputSystem;

namespace Sword
{
    public class Sword : MonoBehaviour
    {
        [Header("Настройки атаки")] public int damage = 25;
        public float attackRange = 1.5f;
        public float attackRate = 1f;
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
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            // Нанесение урона
            //foreach (Collider enemy in hitEnemies)
            //{
            //    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            //    if (enemyHealth != null)
            //    {
            //        enemyHealth.TakeDamage(damage);
            //        Debug.Log("Удар по " + enemy.name + " нанес " + damage + " урона");
            //    }
            //}

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
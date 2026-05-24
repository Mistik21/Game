using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sword
{
    public class Sword : MonoBehaviour
    {
        public Animator Animation;
        [Header("Настройки атаки")] public int damage = 25;
        public float attackRange = 10.5f;
        public float attackRate = 0.3f;
        private float nextAttackTime = 0f;

        [Header("Точка удара")] public Transform attackPoint;
        public LayerMask enemyLayers;

        void Update()
        {
            if (Time.timeScale != 0f)
            {
                Transform parentTransform = transform.parent;
                if (parentTransform)
                {
                    // Атака по левой кнопке мыши
                    if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextAttackTime)
                    {
                        Debug.Log("атака");
                        Attack();
                        PlayAttackAnimation("Hit");
                        nextAttackTime = Time.time + attackRate;
                    }
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
                enemy.GetComponent<NPCScript>().Hp-=damage;
                enemy.GetComponent<NPCScript>().FlashRed(0.1f);
            }
            
        }

        // Визуализация радиуса атаки в редакторе
        void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        // Метод для идеального подгона скорости и запуска
        void PlayAttackAnimation(string clipName)
        {
            // ИСПРАВЛЕНО: Теперь желаемая длительность — это и есть ваш attackRate (в секундах)
            float desiredDuration = attackRate; 

            // Ищем длину самого файла анимации
            float originalClipLength = 1f; 
    
            foreach (var clip in Animation.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    originalClipLength = clip.length;
                    break;
                }
            }

            // Считаем скорость. Теперь если клип длится 1 сек, а кулдаун 0.5 сек:
            // 1.0 / 0.5 = 2.0 (анимация УСККОРИТСЯ в два раза, как вам и нужно!)
            float requiredSpeed = originalClipLength / desiredDuration;
            
            Animation.speed = requiredSpeed;
    
            // Запускаем прямо здесь, когда скорость уже применена
            Animation.Play("Base Layer.Hit", 0, 0f);;
        }
    }
}
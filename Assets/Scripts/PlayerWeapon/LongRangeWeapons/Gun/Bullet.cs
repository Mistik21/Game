using System;
using UnityEngine;


namespace GunPlayer
{
    public class Bullet : MonoBehaviour
    {
        [Header("Настройки пули")] public float damage = 10f; // Урон
        public float lifetime = 3f; // Время жизни пули

        void Start()
        {
            // Уничтожаем пулю через время
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Проверяем, не попала ли пуля в стену
            if (other.CompareTag("Wall") || other.CompareTag("Door"))
            {
                Destroy(gameObject);
            }
    
            // Проверяем, не попала ли пуля во врага
            if (other.CompareTag("Enemy"))
            {
                NPCScript enemy = other.GetComponent<NPCScript>();
                enemy.Hp -= damage;
                enemy.FlashRed(0.1f);  // Добавляем эффект покраснения
                Destroy(gameObject);
            }
        }

        void OnBecameInvisible()
        {
            // Уничтожаем пулю, если вышла за экран
            Destroy(gameObject);
        }
    }
}
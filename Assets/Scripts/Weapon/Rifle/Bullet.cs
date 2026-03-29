using System;
using UnityEngine;


namespace Rifle
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
            // Проверяем, не попала ли пуля в игрока
            if (other.CompareTag("P"))
            {
                return;
            }


            // Уничтожаем пулю
            Destroy(gameObject);
        }

        void OnBecameInvisible()
        {
            // Уничтожаем пулю, если вышла за экран
            Destroy(gameObject);
        }
    }
}
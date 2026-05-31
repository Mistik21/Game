using System;
using UnityEngine;


namespace GunNPC
{
    public class Bullet : MonoBehaviour
    {
        [Header("Настройки пули")] public float damage = 5f; // Урон
        public float lifetime = 3f; // Время жизни пули

        void Start()
        {
            // Уничтожаем пулю через время
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Wall") || other.CompareTag("Door"))
            {
                Destroy(gameObject);
            }
            if (other.CompareTag("Player"))
            {
                PlayerScript player = other.GetComponent<PlayerScript>();
                SoundEffectsManager.Instance.PlayHurt();
                player.Hp -= damage;
                player.FlashRed(0.1f); // 0.1 секунды = 100 миллисекунд
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
using UnityEngine;
using System.Collections;

namespace GunNPC
{
    public class BossWeapon : MonoBehaviour
    {
        [Header("Настройки стрельбы")] 
        public float fireRate = 3f; 
        public float bulletForce = 8f;
        
        [Header("Круговая стрельба")]
        public int bulletsInCircle = 38; 
        
        [Header("Патроны")] 
        public GameObject bulletPrefab;
        public Transform firePoint;
        public int maxAmmo = 30;
        public int currentAmmo;
        
        [Header("Задержки")]
        public float initialReloadDelay = 2.5f;
        public float reloadTime = 2.5f;
        
        private float nextTimeToFire = 0f;
        private bool isReloading = false;
        private bool isInitialDelay = true;
        private NPCScript npcScript;
        private Animator anim; // Добавили ссылку на аниматор

        void Start()
        {
            currentAmmo = maxAmmo;
            if (firePoint == null) firePoint = transform;
            
            npcScript = GetComponent<NPCScript>();
            // Инициализируем аниматор
            anim = GetComponent<Animator>(); 
            
            StartCoroutine(InitialDelay());
        }

        IEnumerator InitialDelay()
        {
            isReloading = true;
            isInitialDelay = true;
            yield return new WaitForSeconds(initialReloadDelay);
            isReloading = false;
            isInitialDelay = false;
        }
        
        void Update()
        {
            if (npcScript == null || isInitialDelay) return;
            
            if (!isReloading && currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            Shoot();
        }
        
        void Shoot()
        {
            if (Time.time < nextTimeToFire || currentAmmo <= 0 || isReloading)
                return;
            
            nextTimeToFire = Time.time + fireRate;
            
            // СПОСОБ 2: Прямое воспроизведение анимации
            if (anim != null)
            {
                // "Attack" — это точное название твоего анимационного клипа в Animator
                // 0 — это индекс базового слоя (Base Layer)
                // 0f — это время, с которого начать (самое начало клипа)
                anim.Play("Attack", 0, 0f); 
            }
            
            int ammoToSpend = Mathf.Min(bulletsInCircle, currentAmmo);
            currentAmmo -= ammoToSpend;
            
            SpawnCirclePattern(ammoToSpend);
        }
        
        void SpawnCirclePattern(int count)
        {
            if (bulletPrefab == null || firePoint == null) return;

            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float currentAngle = i * angleStep;
                
                Vector2 bulletDirection = new Vector2(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                );

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                
                float lookAngle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, lookAngle);
                
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = bulletDirection * bulletForce;
                }
                
                Destroy(bullet, 4f); 
            }
        }
        
        IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadTime);
            currentAmmo = maxAmmo;
            isReloading = false;
        }
    }
}
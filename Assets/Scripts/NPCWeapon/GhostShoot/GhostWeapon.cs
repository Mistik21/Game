using UnityEngine;
using System.Collections;

namespace GunNPC
{
    public class GhostWeapon : MonoBehaviour
    {
        [Header("Настройки стрельбы")] 
        public float fireRate = 10f;
        public float bulletForce = 11.5f;
        
        [Header("Веерная стрельба")]
        public int bulletsPerShot = 10;
        private float spreadAngle = 25f;
        
        [Header("Патроны")] 
        public GameObject bulletPrefab;
        public Transform firePoint;
        public int maxAmmo = 10;
        public int currentAmmo;
        public int ammoPerReload = 10;
        
        [Header("Начальная задержка")]
        public float initialReloadDelay = 1.5f; // Задержка перед первой стрельбой
        
        private float nextTimeToFire = 0f;
        private bool isReloading = false;
        private bool isInitialDelay = true; // Флаг начальной задержки
        private GameObject player;
        private NPCScript npcScript;
        
        void Start()
        {
            currentAmmo = maxAmmo;
            
            if (firePoint == null)
                firePoint = transform;
            
            npcScript = GetComponent<NPCScript>();
            
            // Запускаем начальную задержку
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
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
            
            if (npcScript == null) return;
            
            // Если идет начальная задержка - не стреляем
            if (isInitialDelay) return;
            
            if (!isReloading && currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }
            
            if (npcScript.IsWallBetween(LayerMask.GetMask("Wall", "Obstacle")))
                return;
            
            Shoot();
        }
        
        void Shoot()
        {
            if (Time.time < nextTimeToFire)
                return;
            
            if (currentAmmo <= 0) return;
            
            if (isReloading) return;
            
            nextTimeToFire = Time.time + fireRate;
            
            int ammoToSpend = Mathf.Min(bulletsPerShot, currentAmmo);
            currentAmmo -= ammoToSpend;
            
            SpawnBulletFan();
        }
        
        void SpawnBulletFan()
        {
            if (bulletPrefab == null) return;
            if (firePoint == null || player == null) return;
            
            Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;
            
            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / (bulletsPerShot - 1);
            
            for (int i = 0; i < bulletsPerShot; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Vector2 bulletDirection = RotateVector(directionToPlayer, currentAngle);
                
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                
                float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = bulletDirection * bulletForce;
                }
                
                Destroy(bullet, 3f);
            }
        }
        
        Vector2 RotateVector(Vector2 vector, float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);
            
            return new Vector2(
                vector.x * cos - vector.y * sin,
                vector.x * sin + vector.y * cos
            );
        }
        
        IEnumerator Reload()
        {
            isReloading = true;
            float reloadTime = 1.5f;
            yield return new WaitForSeconds(reloadTime);
            currentAmmo = maxAmmo;
            isReloading = false;
        }
    }
}
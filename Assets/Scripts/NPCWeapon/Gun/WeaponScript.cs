using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


namespace GunNPC
{
    public class WeaponScript : MonoBehaviour
    {
        [Header("Настройки стрельбы")] public float fireRate = 1.3f;
        public float bulletForce = 11.5f;

        [Header("Патроны")] public GameObject bulletPrefab;
        public Transform firePoint;
        public int maxAmmo = 30;
        public int currentAmmo;
        public int ammoPerReload = 30;

        private float nextTimeToFire = 0f;
        private bool isReloading = false;
        private Camera mainCamera;


        void Start()
        {
            currentAmmo = maxAmmo;
            mainCamera = Camera.main;

            if (firePoint == null)
                firePoint = transform;
        }

        void Update()
        {
            Transform parentTransform = transform.parent;
            if (parentTransform)
            {
                if (!isReloading && currentAmmo < maxAmmo)
                {
                    StartCoroutine(Reload());
                    return;
                }

                try
                {
                    if (!parentTransform.GetComponent<NPCScript>().IsWallBetween())
                    {
                        Shoot();
                    }
                }
                catch (Exception)
                {
                    enabled = false;
                }
            }
        }

        void Shoot()
        {
            // Проверка времени выстрела
            if (Time.time < nextTimeToFire)
                return;

            // Проверка патронов
            if (currentAmmo <= 0)
                return;

            // Устанавливаем время следующего выстрела
            nextTimeToFire = Time.time + fireRate;

            // Тратим патрон
            currentAmmo--;

            // Создаем пулю
            SpawnBullet();
        }

        void SpawnBullet()
        {
            if (bulletPrefab == null)
            {
                Debug.LogWarning("Bullet Prefab не назначен!");
                return;
            }

            if (firePoint == null)
                return;

            // ИСПРАВЛЕНО: используем новый Input System
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector2 direction = (player.transform.position - firePoint.position).normalized;

            // Создаем пулю
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Поворачиваем пулю в направлении полета
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Добавляем силу для полета
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletForce;
            }

            // Уничтожаем пулю через 3 секунды
            Destroy(bullet, 3f);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator Reload()
        {
            isReloading = true;


            // Добавляем патроны
            int ammoToAdd = ammoPerReload;
            int ammoToReload = ammoToAdd;
            
            
            // Время перезарядки
            float reloadTime = 1.5f;
            yield return new WaitForSeconds(reloadTime);
            currentAmmo += ammoToReload;
            isReloading = false;
        }

        // Метод для проверки, можно ли стрелять
        bool CanShoot()
        {
            return !isReloading && currentAmmo > 0;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace RiflePlayer
{
    public class WallWeapon : BaseWeapon
    {
        [Header("Настройки веерной стрельбы")]
        public int bulletsPerShot = 10;
        public float spreadAngle = 12f;
        
        [Header("Настройка поворота спрайта")]
        public float visualAngleOffset = -90f;
        
        private float totalMana;
        
        void Start()
        {
            bulletForce = 10f;
            reloadTime = 2f;
            maxAmmo = 20;
            ammoPerReload = 90;
            rotationOffset = 0f;
            maxUpAngle = 70f;
            maxDownAngle = -70f;
            nextTimeToFire = 0f;
            isReloading = false;
            currentWeaponAngle = 0f;
            currentAmmo = maxAmmo;
            mainCamera = Camera.main;
            sale = true;
            type = "M";

            if (firePoint == null) firePoint = transform;
        }

        void Update()
        {
            if (Time.timeScale != 0f)
            {
                Transform parentTransform = transform.parent;
                if (parentTransform && !sale)
                {
                    totalMana = parentTransform.GetComponent<PlayerScript>().Mana;
                    RotateWeaponTowardsMouse();

                    if (currentAmmo == 0 && !isReloading && currentAmmo < maxAmmo && totalMana > 0)
                    {
                        StartCoroutine(GameObject.Find("Inventory").GetComponent<InventoryView>().ReloadRoutine(reloadTime));
                        StartCoroutine(Reload());
                        return;
                    }

                    if (Mouse.current.leftButton.isPressed && !isReloading)
                    {
                        Shoot();
                    }
                }
                else
                {
                    StopAllCoroutines();
                    isReloading = false;
                }
            }
        }

        void RotateWeaponTowardsMouse()
        {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0f;
            Vector2 worldDirection = (mouseWorldPosition - transform.position).normalized;
            bool isParentFlipped = transform.parent != null && transform.parent.localScale.x < 0;
            float angle;

            if (isParentFlipped)
            {
                Vector2 reflectedDirection = new Vector2(-worldDirection.x, worldDirection.y);
                angle = Mathf.Atan2(reflectedDirection.y, reflectedDirection.x) * Mathf.Rad2Deg;
            }
            else
            {
                angle = Mathf.Atan2(worldDirection.y, worldDirection.x) * Mathf.Rad2Deg;
            }

            angle += rotationOffset;
            
            // Для зеркального режима ограничения должны быть другими
            if (isParentFlipped)
            {
                angle = Mathf.Clamp(angle, -maxUpAngle, -maxDownAngle);
            }
            else
            {
                angle = Mathf.Clamp(angle, maxDownAngle, maxUpAngle);
            }
            
            currentWeaponAngle = angle;
            
            float visualAngle = angle + visualAngleOffset;
            transform.localRotation = Quaternion.Euler(0, 0, visualAngle);
        }

        void Shoot()
        {
            if (Time.time < nextTimeToFire)
                return;
            if (currentAmmo <= 0)
                return;
            if (isReloading) return;
            
            nextTimeToFire = Time.time + fireRate;
            int ammoToSpend = Mathf.Min(bulletsPerShot, currentAmmo);
            currentAmmo -= ammoToSpend;
            SpawnBulletFan();
        }

        void SpawnBulletFan()
        {
            if (bulletPrefab == null)
            {
                Debug.LogWarning("Bullet Prefab не назначен!");
                return;
            }
            if (firePoint == null)
                return;

            bool isParentFlipped = transform.parent != null && transform.parent.localScale.x < 0;
            
            // Получаем направление к курсору МЫШИ (а не от поворота оружия)
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0f;
            Vector2 directionToMouse = (mouseWorldPosition - firePoint.position).normalized;
            
            // Базовое направление - всегда на курсор
            Vector2 baseDirection = directionToMouse;
            
            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / (bulletsPerShot - 1);
            
            for (int i = 0; i < bulletsPerShot; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                
                // Создаем веер относительно направления на курсор
                Vector2 bulletDirection = RotateVector(baseDirection, currentAngle);
                
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
            yield return new WaitForSeconds(reloadTime);
            int ammoToAdd = bulletsPerShot;
            int neededAmmo = maxAmmo - currentAmmo;
            int ammoToReload = Mathf.Min(ammoToAdd, neededAmmo);

            transform.parent.GetComponent<PlayerScript>().Mana -= ammoPerReload;
            if (transform.parent.GetComponent<PlayerScript>().Mana < 0)
                transform.parent.GetComponent<PlayerScript>().Mana = 0;
            totalMana = transform.parent.GetComponent<PlayerScript>().Mana;
            currentAmmo += ammoToReload;
            isReloading = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            isReloading = false;
        }
    }
}
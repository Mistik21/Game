using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace GunPlayer
{
    public class WeaponScript : MonoBehaviour
    {
        [Header("Настройки стрельбы")] 
        public float fireRate = 0.15f;
        public float bulletForce = 20f;
        public float ReloadTime = 1f;

        [Header("Патроны")] 
        public GameObject bulletPrefab;
        public Transform firePoint;
        public int maxAmmo = 30;
        public int currentAmmo;
        private int totalAmmo;
        public int ammoPerReload = 30;

        [Header("Настройки оружия")]
        public float rotationOffset = 0f;
        public float maxUpAngle = 70f;
        public float maxDownAngle = -70f;

        private float nextTimeToFire = 0f;
        private bool isReloading = false;
        private Camera mainCamera;
        private float currentWeaponAngle = 0f;

        void Start()
        {
            currentAmmo = maxAmmo;
            mainCamera = Camera.main;

            if (firePoint == null)
                firePoint = transform;
        }

        void Update()
        {
            if (Time.timeScale != 0f)
            {
                Transform parentTransform = transform.parent;
                if (parentTransform)
                {
                    totalAmmo = parentTransform.GetComponent<PlayerScript>().Ammo;
                    
                    RotateWeaponTowardsMouse();
                    
                    if ((Keyboard.current.rKey.wasPressedThisFrame || currentAmmo==0) && !isReloading && currentAmmo < maxAmmo && totalAmmo > 0)
                    {
                        StartCoroutine(GameObject.Find("Inventory").GetComponent<InventoryView>().ReloadRoutine(ReloadTime));
                        StartCoroutine(Reload());
                        return;
                    }

                    if (Mouse.current.leftButton.wasPressedThisFrame && !isReloading)
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
            // Получаем позицию мыши в мировых координатах
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0f;

            // Получаем направление от оружия к мыши
            Vector2 worldDirection = (mouseWorldPosition - transform.position).normalized;
            
            // Проверяем зеркалирование РОДИТЕЛЯ
            bool isParentFlipped = transform.parent != null && transform.parent.localScale.x < 0;
            
            float angle;
            
            if (isParentFlipped)
            {
                // Если родитель зеркален, используем отражённое направление для поворота
                Vector2 reflectedDirection = new Vector2(-worldDirection.x, worldDirection.y);
                angle = Mathf.Atan2(reflectedDirection.y, reflectedDirection.x) * Mathf.Rad2Deg;
            }
            else
            {
                angle = Mathf.Atan2(worldDirection.y, worldDirection.x) * Mathf.Rad2Deg;
            }
            
            // Добавляем смещение
            angle += rotationOffset;
            
            // Ограничиваем угол
            angle = Mathf.Clamp(angle, maxDownAngle, maxUpAngle);
            
            // Сохраняем угол для стрельбы
            currentWeaponAngle = angle;
            
            // Применяем поворот
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        void Shoot()
        {
            if (Time.time < nextTimeToFire)
                return;

            if (currentAmmo <= 0)
                return;

            nextTimeToFire = Time.time + fireRate;
            currentAmmo--;
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

            // Проверяем зеркалирование родителя
            bool isParentFlipped = transform.parent != null && transform.parent.localScale.x < 0;
    
            Vector2 direction;
    
            if (isParentFlipped)
            {
                // При зеркалировании пуля летит налево (противоположное направление)
                direction = -firePoint.right;
            }
            else
            {
                // Обычное состояние - пуля летит направо
                direction = firePoint.right;
            }

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
        IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(ReloadTime);
            int ammoToAdd = Mathf.Min(ammoPerReload, totalAmmo);
            int neededAmmo = maxAmmo - currentAmmo;
            int ammoToReload = Mathf.Min(ammoToAdd, neededAmmo);

            transform.parent.GetComponent<PlayerScript>().Ammo -= ammoToReload;

            if (transform.parent.GetComponent<PlayerScript>().Ammo < 0)
                transform.parent.GetComponent<PlayerScript>().Ammo = 0;
            totalAmmo = transform.parent.GetComponent<PlayerScript>().Ammo;
            
            currentAmmo += ammoToReload;
            isReloading = false;
        }

        void AddAmmo(int amount)
        {
            totalAmmo += amount;
        }

        string GetAmmoInfo()
        {
            return currentAmmo + " / " + totalAmmo;
        }

        bool CanShoot()
        {
            return !isReloading && currentAmmo > 0;
        }
        private void OnDisable()
        {
            // Останавливаем все корутины при отключении объекта
            StopAllCoroutines();
            isReloading = false;
        }
    }
}
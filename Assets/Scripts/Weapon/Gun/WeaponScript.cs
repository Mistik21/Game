using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    [Header("Настройки стрельбы")] public float fireRate = 0.2f;
    public float bulletForce = 20f;

    [Header("Патроны")] public GameObject bulletPrefab;
    public Transform firePoint;
    public int maxAmmo = 12;
    public int currentAmmo;
    public int totalAmmo = 60;
    public int ammoPerReload = 12;

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
        // Перезарядка по R
        if (Keyboard.current.rKey.wasPressedThisFrame && !isReloading && currentAmmo < maxAmmo && totalAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Стрельба по ЛКМ
        if (Mouse.current.leftButton.wasPressedThisFrame && !isReloading)
        {
            Shoot();
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
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = (mouseWorldPosition - firePoint.position).normalized;

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

        // Время перезарядки
        float reloadTime = 1.5f;
        yield return new WaitForSeconds(reloadTime);

        // Добавляем патроны
        int ammoToAdd = Mathf.Min(ammoPerReload, totalAmmo);
        int neededAmmo = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoToAdd, neededAmmo);

        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        if (totalAmmo < 0) totalAmmo = 0;

        isReloading = false;
    }

    // Метод для добавления патронов
    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
    }

    // Метод для получения информации о патронах
    public string GetAmmoInfo()
    {
        return currentAmmo + " / " + totalAmmo;
    }

    // Метод для проверки, можно ли стрелять
    public bool CanShoot()
    {
        return !isReloading && currentAmmo > 0;
    }
}
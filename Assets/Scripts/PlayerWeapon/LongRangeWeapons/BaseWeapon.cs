using Unity.VisualScripting;
using UnityEngine;

namespace RiflePlayer
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [Header("Настройки стрельбы")] public float fireRate;

        public float bulletForce;
        public float reloadTime;

        [Header("Патроны")] public GameObject bulletPrefab;

        public Transform firePoint;
        public int maxAmmo;
        public int currentAmmo;
        public int totalAmmo;
        public int ammoPerReload ;

        [Header("Настройки оружия")] public float rotationOffset;

        public float maxUpAngle;
        public float maxDownAngle;
        public float nextTimeToFire;
        public bool isReloading;
        public Camera mainCamera;
        public float currentWeaponAngle;
        public bool sale;
        public int price;
        public GameObject typeView;
        public string type;
        public float[] scl;
    }
}
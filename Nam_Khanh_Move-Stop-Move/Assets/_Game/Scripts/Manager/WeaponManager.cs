//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WeaponManager : MonoBehaviour
//{
//    public WeaponType currentWeapon;
//    private ObjectPool projectilePool;

//    [SerializeField] private Transform weaponHolder; // v? trí tay c?m
//    private GameObject currentWeaponModel;

//    private void Start()
//    {
//        Equip(currentWeapon);
//    }

//    public void Equip(WeaponType weapon)
//    {
//        currentWeapon = weapon;

//        // Spawn weapon visual
//        if (currentWeaponModel != null)
//            Destroy(currentWeaponModel);

//        if (weapon.weaponPrefab != null)
//        {
//            currentWeaponModel = Instantiate(weapon.weaponPrefab, weaponHolder);
//        }

//        // Create pool for projectile
//        projectilePool = ObjectPool.CreateInstance(
//            weapon.projectilePrefab,
//            Mathf.CeilToInt((1 / weapon.attackDelay) * 5)
//        );
//    }

//    /// <summary>
//    /// B?n ??n h??ng v? target
//    /// </summary>
//    public void Fire(Transform shooter, Transform target)
//    {
//        if (projectilePool == null) return;

//        PoolableObject obj = projectilePool.GetObject();
//        if (obj == null) return;

//        Projectile proj = obj.GetComponent<Projectile>();

//        Vector3 dir = (target.position - shooter.position).normalized;

//        proj.transform.position = shooter.position;
//        proj.Init(dir, currentWeapon.damage, currentWeapon.projectileSpeed);
//    }
//}


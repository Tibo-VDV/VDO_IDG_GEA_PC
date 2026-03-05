using UnityEngine;

[CreateAssetMenu(fileName = "WeaponObject", menuName ="Inventory/create new Weapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType { ScopeRifle, Shotgun, PlasmaRifle, GrenadeLauncher }

    [Header("Weapon type")]
    public WeaponType weaponType = WeaponType.ScopeRifle;

    [Header("Weapon info")]
    public int maxAmmo;
    public int currentAmmo;
    public float fireDelay;

    [Header("Weapon graphics and object")]
    public Sprite weaponSprite;
    public GameObject weaponPrefab;

    [Header("projectile settings")]
    public int projectileDamage;
    public enum ProjectileType { Raycast, physicalProjectile }
    public ProjectileType projectileType = ProjectileType.Raycast;
    public float maxRayDistance = Mathf.Infinity;
    public GameObject physicalProjectile;
}

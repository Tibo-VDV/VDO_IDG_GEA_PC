using Unity.VisualScripting;
using UnityEngine;

public class WeaponLogic: MonoBehaviour
{
    //bevat alle data die aangepast moet worden op ons wapen. hoeft de info niet van het inventory
    //te krijgen. want dit is het object zelf al. zorg wel dat onze weapon
    [Header("Required")]
    [SerializeField] WeaponSO weaponInfo;
    [SerializeField] Transform ProjectileOrigin;

    [Header("UI Reference")]
    [SerializeField] UIDataExample uIDataExample;

    void UpdateUI()
    {
        uIDataExample.UpdateAmmoCountUI(weaponInfo.currentAmmo, weaponInfo.maxAmmo);
    }

    public void FireBullet()
    {
        switch(weaponInfo.projectileType)
        {
            case WeaponSO.ProjectileType.physicalProjectile:
                FireProjectile(weaponInfo.physicalProjectile);
                break;
            case WeaponSO.ProjectileType.Raycast:
                fireHitScan(weaponInfo.maxRayDistance, weaponInfo.hitLayers);
                break;
        }
    }

    public void FireProjectile(GameObject projectile)
    {
        GameObject projectileClone = Instantiate(projectile, ProjectileOrigin.position, ProjectileOrigin.rotation);
        //met projectileClone.Getcomponent kunnen we kijken of de component null returned. zo ja skip de lijn
        projectileClone.GetComponent<BulletMovement>().SetVelocity(weaponInfo.projectileVelocity);
        projectileClone.GetComponent<ProjectileDamage>().SetWeaponInfo(weaponInfo);
        print("Coole projectile goes pfeeeeuw");
    }
    
    public void fireHitScan(float distance, LayerMask hitLayers)
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(ProjectileOrigin.position, ProjectileOrigin.forward);
        if (Physics.Raycast(ray, out hit, distance, hitLayers))
        {
            Debug.DrawRay(ProjectileOrigin.position, ProjectileOrigin.forward * 1000, Color.red, 0.5f);
            print(hit.collider.name);
            if (hit.collider.GetComponent<IDamagable>() == null) return;
            hit.collider.GetComponent<IDamagable>().DoDamage(weaponInfo.projectileDamage);
        }
    }
    
    public void SubtractAmmo()
    {

        int tempAmmo = weaponInfo.currentAmmo;
        tempAmmo -= weaponInfo.ammoCost;
        tempAmmo = Mathf.Clamp(tempAmmo, 0, weaponInfo.maxAmmo);
        weaponInfo.currentAmmo = tempAmmo;
        UpdateUI();
    }
}

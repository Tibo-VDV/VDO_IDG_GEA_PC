using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    WeaponSO weaponInfo;

    void CollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<IDamagable>() == null) return;
        col.gameObject.GetComponent<IDamagable>().DoDamage(weaponInfo.projectileDamage);
    }

    public void SetWeaponInfo(WeaponSO info)
    {
        weaponInfo = info;
    }
}

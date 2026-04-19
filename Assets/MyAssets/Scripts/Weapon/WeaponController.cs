using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponItem weaponItem;

    [SerializeField] WeaponAnimationController getWeaponAnimationController;
    PlayerController getPlayerController => GetComponent<PlayerController>();

    bool initialized = false;
    

    void LateUpdate()
    {
        if (getWeaponAnimationController == null)
        {
            //print("No reference to WeaponAnimationController, assign reference in inspector or check if weapon is assigned correctly");
            return;
        }
        getWeaponAnimationController.SetMoveSpeed(getPlayerController.currentSpeed);
    }

    public void UpdateWeapon(WeaponItem weapon)
    {
        weaponItem = weapon;
        InitializeWeapon();
    }

    void InitializeWeapon()
    {
        if (weaponItem == null)
        {
            Debug.LogError("no reference to weaponItem, check if inventory set it correctly");
            return;
        }
        getWeaponAnimationController = weaponItem.weaponGameObject.GetComponent<WeaponAnimationController>();
        initialized = true;
        return;

    }

    

    public void OnAttack(InputValue context)
    {
        if (getWeaponAnimationController == null)
        {
            Debug.LogError("No AnimationController found on weaponObject");
            return;
        }
        if(!initialized) return;
        if (getWeaponAnimationController.GetFireState()) return; //Als we al aan het schieten zijn, returnen we zodat we niet opnieuw kunnen schieten voordat de animatie klaar is.
        getWeaponAnimationController.FireWeapon(1);//1 = true, 0 = false
        
    }





}

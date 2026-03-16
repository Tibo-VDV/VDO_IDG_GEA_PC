using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [SerializeField] GameObject weaponObject;
    [SerializeField] WeaponAnimationController getWeaponController;
    PlayerController getPlayerController => GetComponent<PlayerController>();
    

    void LateUpdate()
    {
        getWeaponController.SetMoveSpeed(getPlayerController.currentSpeed);
    }

    public void UpdateWeapon(GameObject Weapon)
    {
        weaponObject = Weapon;
        InitializeWeapon();
    }

    private bool InitializeWeapon()
    {
        if (weaponObject == null)
        {
            Debug.LogError("no reference to weaponObject, check if inventory set it correctly");
            return false;
        }
        getWeaponController = weaponObject.GetComponent<WeaponAnimationController>();
        return true;

    }

    public void OnAttack(InputValue context)
    {
        if (getWeaponController == null)
        {
            Debug.LogError("No AnimationController found on weaponObject");
            return;
        }

        if (getWeaponController.GetFireState()) return; //Als we al aan het schieten zijn, returnen we zodat we niet opnieuw kunnen schieten voordat de animatie klaar is.
        getWeaponController.FireWeapon(1);//1 = true, 0 = false
    }

   

}

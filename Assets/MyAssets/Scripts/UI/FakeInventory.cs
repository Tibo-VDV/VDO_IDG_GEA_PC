using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FakeInventory : MonoBehaviour
{
    // [] maakt het een lijst in unity
    //[SerializeField] WeaponUI[] weapon;
    //[SerializeField] WeaponUIStruct[] weaponStruct;
    [SerializeField] WeaponSO[] weaponSO;
    [SerializeField] WeaponSO selectedWeapon;
    [SerializeField] int index;

    [Header("UI Reference")]
    [SerializeField] UIDataExample uIDataExample;

    void Start()
    {
        index = 0;
        selectedWeapon = weaponSO[index];
        uIDataExample.OnInitializeSO(selectedWeapon);
    }
    
    void OnScrollWheel(InputValue value)
    {
        float scrollDirection = value.Get<float>();
        index += (int)scrollDirection;
        index = index % weaponSO.Length; // loopend scollijst zonder if statements via gebruik wiskundige formule "modulo" gebruikt door % te typen
        index = index < 0 ? weaponSO.Length - 1 : index;
        selectedWeapon = weaponSO[index];
        Debug.Log($"Selected weapon: {selectedWeapon.weaponType}");
        uIDataExample.OnInitializeSO(selectedWeapon);


        
    }

}
#region Class&Struct
// we maken een eige class, oftewel "object" aan. hiering geven we properties/eigenschappen die dit object beschrijft mee.
//Dit object kan nu als een reference type gebruikt worden
// [serializable] boven een class die we zelf maken zorgt dat de public variables zichtbaar zijn in unity inspector
[Serializable]
public class WeaponUI   // class is een reference type. Wanneer nieuwe variables dezelfde type WeaponUI gemaakt worden en we deze instellen met =.
// Dan point de nieuwe variable altijd naar het origineel.
{
    public int maxAmmo;
    public int currentAmmo;
    public float fireDelay;
    public Sprite weaponSprite;

}
[Serializable]
public struct WeaponUIStruct    // struct is een value type. Wanneer we nieuwe variables van dezelde struct weaponUIStruct
//gemaakt worden en we deze instellen met =. Dan point deze NIET naar het origineel. Dit wordt echter een unieke
//copie waar de data naar overgeschereven word en beiden worden hun eigen entiteit in memory.
{
    public int maxAmmo;
    public int currentAmmo;
    public float fireDelay;
    public Sprite weaponSprite;
}
#endregion
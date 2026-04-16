using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FakeInventory : MonoBehaviour
{
    // [] maakt het een lijst in unity
    //[SerializeField] WeaponUI[] weapon;
    //[SerializeField] WeaponUIStruct[] weaponStruct;
    [Header("inventory")]
    [SerializeField] WeaponItem[] weapons;
    [SerializeField] WeaponItem selectedWeapon;
    [SerializeField] int index;
    /*
    [Header("Inventory object reference")]
    [SerializeField] GameObject inventoryObject;
    */
    [Header("UI Reference")]
    [SerializeField] UIDataExample uIDataExample;

    WeaponController getWeaponController => GetComponent<WeaponController>();
    PlayerController getPlayerController => GetComponent<PlayerController>();
    bool initialized = false; //zeker dat alle info correct gehaald word als dit niet is krijgen we de error en gaat die niet verder

    static public FakeInventory instance;

    void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start()
    {
        /*
        if(inventoryObject == null)
        {
            Debug.LogError("No reference to inventoryObject, assign reference.");
            return;
        }
        */
        index = 0;
        selectedWeapon = weapons[index];
        //children = inventoryObject.GetComponentsInChildren<Transform>();
        InitializeInventoryItems();
        initialized = true;
    }

    void OnScrollWheel(InputValue value)
    {
        if (!initialized) return;
        float scrollDirection = value.Get<float>();
        index += (int)scrollDirection;
        index = index % weapons.Length; // loopend scollijst zonder if statements via gebruik wiskundige formule "modulo" gebruikt door % te typen
        index = index < 0 ? weapons.Length - 1 : index;
        selectedWeapon = weapons[index];
        //Debug.Log($"Selected weapon: {selectedWeapon.weaponInfo.weaponType}");
        InitializeInventoryItems(); // Initialize altijd eerst, anders kunnen de regels hieronder niet bij hun data
    }

    // zet onze selected item aan en alle rest uit
    void InitializeInventoryItems()
    {
        selectedWeapon.weaponGameObject.SetActive(false);
        /*
        int childCount = inventoryObject.childCount;
        for (int i = 0; i < childCount; i++) // alleen < gebruiken omdat index start met 0 niet met 1 anders moet "<= childCount -1"
        {
            print(children[i].name);
        }
        */
        foreach (WeaponItem weaponItem in weapons)
        {
            if (weaponItem == selectedWeapon)
            {
                if (selectedWeapon.weaponInfo.pickedUp)
                    weaponItem.weaponGameObject.SetActive(true);
                continue;   // continue zorgt ervoor dat de rest van de code in deze loop (foreachloop) niet word uitgevoerd en meteen naar de volgende iteratie gaat.
                            // zo voorkomen we dat we onze geselecteerde weapon meteen weer uitzetten.
            }
            weaponItem.weaponGameObject.SetActive(false);
        }
        uIDataExample.OnInitializeSO(selectedWeapon.weaponInfo);
        getWeaponController.UpdateWeapon(selectedWeapon);
        getPlayerController.UpdateWeapon(selectedWeapon);
    }
    
    public void PickUpItem(WeaponSO item)
    {
        
        int _index = 0;
        foreach (WeaponItem weaponItem in weapons)
        {

            if (weaponItem.weaponInfo == item)
            {
                print("inIfstate");
                weaponItem.weaponInfo.pickedUp = true;
                selectedWeapon = weaponItem;
                index = _index;
                break;
                
            }
            _index++;
        }
        InitializeInventoryItems();
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
#region ScriptableObject + GameObject wrapper 
[Serializable]
public class WeaponItem
{
    public WeaponSO weaponInfo;
    public GameObject weaponGameObject;
}
#endregion
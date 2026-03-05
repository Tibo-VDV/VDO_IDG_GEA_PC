using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDataExample : MonoBehaviour
{
    
    [SerializeField] Slider getSlider;
    [SerializeField] Image getImage;
    [SerializeField] TMP_Text getText;

    /*IEnumerator AmmoTest()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireDelay);
            currentAmmo--;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, 30);
            // met de $ notatie kunnen we veriales meteen als string interpreteren door ze te ecapsuleren{}
            //:D2 is hetzelfde als int.ToString("00") locked cijfers op 2 decimalen of hoeveel nullen je wilt
            string combinedText = $"{currentAmmo:D2}/{maxAmmo:D2}";
            getText.SetText(combinedText);
            getSlider.value = currentAmmo;
            yield return null;
        }
    }
    */

    /*void InitializeUIValues(int _maxAmmo, int _currentAmmo, float _fireDelay, Sprite _weaponSprite)
    {
        maxAmmo = _maxAmmo;
        currentAmmo = maxAmmo == 0 ? maxAmmo : _currentAmmo;
        getSlider.maxValue = maxAmmo;
        getSlider.value = currentAmmo;
        getImage.sprite = _weaponSprite;
        fireDelay = _fireDelay;
    }*/

    public void OnInitializeSO(WeaponSO _weaponSO)
    {
        if (_weaponSO == null)
        {
            Debug.LogError("WeaponSO is null, cannot initialize UI values.");
            return;
        }
        UpdateUI(_weaponSO.maxAmmo, _weaponSO.currentAmmo, _weaponSO.weaponSprite);

    }

    void UpdateUI(int _currentAmmo, int _maxAmmo, Sprite _weaponSprite)
    {

        if (_weaponSprite != null) getImage.sprite = _weaponSprite;
        UpdateAmmoCountUI(_currentAmmo, _maxAmmo);
    }
    
    public void UpdateAmmoCountUI(int _currentAmmo,int _maxAmmo)
    {
        string combinedText = $"{_currentAmmo:D2}/{_maxAmmo:D2}";
        getText.SetText(combinedText);
        
        getSlider.maxValue = _maxAmmo;
        getSlider.value = _currentAmmo;
    }
    

}

using UnityEngine;
using System.Collections;

public class WeaponSelection : MonoBehaviour {

    public string weaponType;
    public GameObject[] weaponModels;
	
    public void WeaponButton() //TODO change logs to the appropriate weapon selection, once I have the definite name of them in the Settings class
    {
        if (weaponType == "pistol")
        {
            Settings.weaponType = WeaponType.ScatterGun;
            weaponModels[0].SetActive(true);
            weaponModels[1].SetActive(false);
            weaponModels[2].SetActive(false);
          //  weaponModels[3].SetActive(false);
        }
        else if (weaponType == "water can")
        {
            Settings.weaponType = WeaponType.ScatterGun;
            weaponModels[0].SetActive(false);
            weaponModels[1].SetActive(true);
            weaponModels[2].SetActive(false);
          //  weaponModels[3].SetActive(false);
        }
            
        else if (weaponType == "rake")
        {
            Settings.weaponType = WeaponType.WaterRake;
            weaponModels[0].SetActive(false);
            weaponModels[1].SetActive(false);
            weaponModels[2].SetActive(true);
          //  weaponModels[3].SetActive(false);
        }
        
        //TODO add the new weapon

    }
}

using UnityEngine;
using System.Collections;

public class WeaponSelection : MonoBehaviour {

    public WeaponType weaponType;
    public GameObject[] weaponModels;
	
    public void WeaponButton() //TODO change logs to the appropriate weapon selection, once I have the definite name of them in the Settings class
    {
        Settings.weaponType = weaponType;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResButton : MonoBehaviour {

    public GameObject optionsMenuObject;
    OptionsMenu optionsMenu;
    Dropdown dropDownMenu;

    void Start()
    {
        dropDownMenu = GetComponent<Dropdown>();
        optionsMenu = optionsMenuObject.GetComponent<OptionsMenu>();
    }

    public void ResolutionButton()
    {
        optionsMenu.SetResolution(dropDownMenu.value);
    }
}

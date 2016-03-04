using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayModeButton : MonoBehaviour {

    public bool isFullScreen;

    public GameObject optionsMenuObject;
    OptionsMenu optionsMenu;
    Dropdown dropDownMenu;

    void Start()
    {
        dropDownMenu = GetComponent<Dropdown>();
        optionsMenu = optionsMenuObject.GetComponent<OptionsMenu>();
        if (Screen.fullScreen)
            dropDownMenu.value = 0;
        else
            dropDownMenu.value = 1;
    }

    public void DisplayButton()
    {
        if (dropDownMenu.value == 0)
            isFullScreen = true;
        else
            isFullScreen = false;
        optionsMenu.SetFullScreen(dropDownMenu);
    }
}

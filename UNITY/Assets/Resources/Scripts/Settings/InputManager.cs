using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Use buttons varible and add buttons or remove them then use methods to check if they are being pressed
/// </summary>
public class InputManager
{
    public static List<Button> buttons = new List<Button>();

    public static void LoadDefaults()
    {
        buttons.Add(new Button("Vertical", KeyCode.W, KeyCode.S));
        buttons.Add(new Button("Horizontal", KeyCode.A, KeyCode.D));
    }

    #region Public Methods

    /// <summary>
    /// Use this for joystick or mouse controls. returns a float value between -1, 1
    /// </summary>
    /// <param name="name">Name of the axies</param>
    /// <returns>float (-1 to 1)</returns>
    public static float GetAxies(string name)
    {
        return Input.GetAxis(name);
    }

    /// <summary>
    /// If positive is pressed returns 1, if negative is pressed returns -1 otherwise returns 0
    /// </summary>
    /// <param name="name">name of the axies</param>
    /// <returns>returns int depending on what key is pressed</returns>
    public static int GetButtonAxies(string name)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].name == name)
            {
                if (Input.GetKey(buttons[i].positive))
                {
                    return 1;
                }
                else if (Input.GetKey(buttons[i].negative))
                {
                    return -1;
                }
                else
                {
                    break;
                }
            }
        }

        return 0;
    }

    /// <summary>
    /// Check if key is pressed
    /// </summary>
    /// <param name="name">name of the button</param>
    /// <returns>True or False if button is pressed</returns>
    public static bool GetButtonDown(string name)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].name == name)
            {
                if (Input.GetKeyDown(buttons[i].positive) || Input.GetKeyDown(buttons[i].negative))
                {
                    return true;
                }
                else
                {
                    break;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Check if key is released
    /// </summary>
    /// <param name="name">name of the button</param>
    /// <returns>True or False if button is released</returns>
    public static bool GetButtonUp(string name)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].name == name)
            {
                if (Input.GetKeyUp(buttons[i].positive) || Input.GetKeyUp(buttons[i].negative))
                {
                    return true;
                }
                else
                {
                    break;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Check if key is being pressed
    /// </summary>
    /// <param name="name">name of the button</param>
    /// <returns>True or False if button is being pressed</returns>
    public static bool GetButton(string name)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].name == name)
            {
                if (Input.GetKey(buttons[i].positive) || Input.GetKey(buttons[i].negative))
                {
                    return true;
                }
                else
                {
                    break;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Change Button keys
    /// </summary>
    /// <param name="name">Name of button</param>
    /// <param name="positive">positive key / primary key</param>
    public static void ChangeButton(string name, KeyCode positive)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].positive = positive;
    }

    /// <summary>
    /// Change Button keys
    /// </summary>
    /// <param name="name">Name of button</param>
    /// <param name="positive">positive key / primary key</param>
    /// <param name="negative">negative key / alternative key</param>
    public static void ChangeButton(string name, KeyCode positive, KeyCode negative)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].positive = positive;
            buttons[i].negative = negative;
        }
    }

    #endregion
}
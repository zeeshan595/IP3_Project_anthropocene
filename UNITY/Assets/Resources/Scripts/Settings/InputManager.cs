using UnityEngine;

#region ENUMS

public enum ControllerAxies
{
    LeftStickX,
    LeftStickY,
    RightStickX,
    RightStickY,
    LeftTrigger,
    RightTrigger,
    DPadX,
    DPadY,
    MouseX,
    MouseY
}

public enum ControllerButtons
{
    A,
    B,
    X,
    Y,
    LB,
    RB,
    Select,
    Start,
    LeftStick,
    RightStick,
    DPadUp,
    DPadDown,
    DPadLeft,
    DPadRight
}

#endregion

public class InputManager
{
    #region Axies

    public static float GetAxies(ControllerAxies axies)
    {
        float val = 0;
        switch(axies)
        {
            case ControllerAxies.LeftStickX:
            case ControllerAxies.LeftStickY:
            case ControllerAxies.MouseX:
            case ControllerAxies.MouseY:
                val = Input.GetAxis(axies.ToString());
                break;
            case ControllerAxies.RightStickX:
                if (WindowsCheck())
                    val = Input.GetAxis("4");
                else if (OSXCheck())
                    val = Input.GetAxis("3");
                else
                    val = Input.GetAxis("4");
                break;
            case ControllerAxies.RightStickY:
                if (WindowsCheck())
                    val = Input.GetAxis("5");
                else if (OSXCheck())
                    val = Input.GetAxis("4");
                else
                    val = Input.GetAxis("5");
                break;
            case ControllerAxies.DPadX:
                if (WindowsCheck())
                    val = Input.GetAxis("6");
                else if (OSXCheck())
                    val = 0;
                else
                    val = Input.GetAxis("7");
                break;
            case ControllerAxies.DPadY:
                if (WindowsCheck())
                    val = Input.GetAxis("7");
                else if (OSXCheck())
                    val = 0;
                else
                    val = Input.GetAxis("8");
                break;
            case ControllerAxies.LeftTrigger:
                if (WindowsCheck())
                    val = Input.GetAxis("9");
                else if (OSXCheck())
                    val = Input.GetAxis("5");
                else
                    val = Input.GetAxis("3");
                break;
            case ControllerAxies.RightTrigger:
                if (WindowsCheck())
                    val = Input.GetAxis("10");
                else if (OSXCheck())
                    val = Input.GetAxis("6");
                else
                    val = Input.GetAxis("6");
                break;
            default:
                Debug.Log("Couldn't find controller axies");
                break;
        }
        return val;
    }

    #endregion

    #region Buttons

    public static float ButtonToAxies(KeyCode positive, KeyCode negative)
    {
        if (Input.GetKey(positive))
        {
            return 1;
        }
        else if (Input.GetKey(negative))
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static bool GetButton(ControllerButtons button)
    {
        return Input.GetKey(ConvertToKeyCode(button));
    }

    public static bool GetButtonDown(ControllerButtons button)
    {
        return Input.GetKeyDown(ConvertToKeyCode(button));
    }

    public static bool GetButtonUp(ControllerButtons button)
    {
        return Input.GetKeyUp(ConvertToKeyCode(button));
    }

    #endregion

    #region Private

    private static KeyCode ConvertToKeyCode(ControllerButtons button)
    {
        KeyCode rtn;
        switch (button)
        {
            case ControllerButtons.A:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button0;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button16;
                else
                    rtn = KeyCode.Joystick1Button0;
                break;
            case ControllerButtons.B:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button1;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button17;
                else
                    rtn = KeyCode.Joystick1Button1;
                break;
            case ControllerButtons.X:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button2;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button18;
                else
                    rtn = KeyCode.Joystick1Button2;
                break;
            case ControllerButtons.Y:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button3;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button19;
                else
                    rtn = KeyCode.Joystick1Button3;
                break;
            case ControllerButtons.LB:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button4;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button13;
                else
                    rtn = KeyCode.Joystick1Button4;
                break;
            case ControllerButtons.RB:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button5;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button14;
                else
                    rtn = KeyCode.Joystick1Button5;
                break;
            case ControllerButtons.Select:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button6;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button10;
                else
                    rtn = KeyCode.Joystick1Button6;
                break;
            case ControllerButtons.Start:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button7;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button9;
                else
                    rtn = KeyCode.Joystick1Button7;
                break;
            case ControllerButtons.LeftStick:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button8;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button11;
                else
                    rtn = KeyCode.Joystick1Button9;
                break;
            case ControllerButtons.RightStick:
                if (WindowsCheck())
                    rtn = KeyCode.Joystick1Button9;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button12;
                else
                    rtn = KeyCode.Joystick1Button10;
                break;
            case ControllerButtons.DPadUp:
                if (WindowsCheck())
                    rtn = KeyCode.None;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button5;
                else
                    rtn = KeyCode.Joystick1Button13;
                break;
            case ControllerButtons.DPadDown:
                if (WindowsCheck())
                    rtn = KeyCode.None;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button6;
                else
                    rtn = KeyCode.Joystick1Button14;
                break;
            case ControllerButtons.DPadLeft:
                if (WindowsCheck())
                    rtn = KeyCode.None;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button7;
                else
                    rtn = KeyCode.Joystick1Button11;
                break;
            case ControllerButtons.DPadRight:
                if (WindowsCheck())
                    rtn = KeyCode.None;
                else if (OSXCheck())
                    rtn = KeyCode.Joystick1Button8;
                else
                    rtn = KeyCode.Joystick1Button12;
                break;
            default:
                Debug.Log("Button not mapped");
                rtn = KeyCode.None;
                break;
        }

        return rtn;
    }

    private static bool WindowsCheck()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsWebPlayer;
    }

    private static bool OSXCheck()
    {
        return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer;
    }

    #endregion
}
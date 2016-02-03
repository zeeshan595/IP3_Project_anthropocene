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

    public float GetAxies(ControllerAxies axies)
    {
        float val = 0;
        switch(axies)
        {
            case ControllerAxies.LeftStickX:
            case ControllerAxies.LeftStickY:
                val = Input.GetAxis(axies.ToString());
                break;
            case ControllerAxies.RightStickX:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    val = Input.GetAxis("4");
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    val = Input.GetAxis("3");
                else
                    val = Input.GetAxis("4");
                break;
            case ControllerAxies.RightStickY:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    val = Input.GetAxis("5");
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    val = Input.GetAxis("4");
                else
                    val = Input.GetAxis("5");
                break;
            case ControllerAxies.DPadX:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    val = Input.GetAxis("6");
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    val = 0;
                else
                    val = Input.GetAxis("7");
                break;
            case ControllerAxies.DPadY:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    val = Input.GetAxis("7");
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    val = 0;
                else
                    val = Input.GetAxis("8");
                break;
            case ControllerAxies.LeftTrigger:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    val = Input.GetAxis("9");
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    val = Input.GetAxis("5");
                else
                    val = Input.GetAxis("3");
                break;
            case ControllerAxies.RightTrigger:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    val = Input.GetAxis("10");
                else if (Application.platform == RuntimePlatform.OSXPlayer)
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

    public bool GetButton(ControllerButtons button)
    {
        return Input.GetKey(ConvertToKeyCode(button));
    }

    public bool GetButtonDown(ControllerButtons button)
    {
        return Input.GetKeyDown(ConvertToKeyCode(button));
    }

    public bool GetButtonUp(ControllerButtons button)
    {
        return Input.GetKeyUp(ConvertToKeyCode(button));
    }

    #endregion

    #region Private

    private KeyCode ConvertToKeyCode(ControllerButtons button)
    {
        KeyCode rtn;
        switch (button)
        {
            case ControllerButtons.A:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button0;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button16;
                else
                    rtn = KeyCode.Joystick1Button0;
                break;
            case ControllerButtons.B:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button1;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button17;
                else
                    rtn = KeyCode.Joystick1Button1;
                break;
            case ControllerButtons.X:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button2;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button18;
                else
                    rtn = KeyCode.Joystick1Button2;
                break;
            case ControllerButtons.Y:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button3;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button19;
                else
                    rtn = KeyCode.Joystick1Button3;
                break;
            case ControllerButtons.LB:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button4;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button13;
                else
                    rtn = KeyCode.Joystick1Button4;
                break;
            case ControllerButtons.RB:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button5;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button14;
                else
                    rtn = KeyCode.Joystick1Button5;
                break;
            case ControllerButtons.Select:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button6;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button10;
                else
                    rtn = KeyCode.Joystick1Button6;
                break;
            case ControllerButtons.Start:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button7;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button9;
                else
                    rtn = KeyCode.Joystick1Button7;
                break;
            case ControllerButtons.LeftStick:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button8;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button11;
                else
                    rtn = KeyCode.Joystick1Button9;
                break;
            case ControllerButtons.RightStick:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.Joystick1Button9;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button12;
                else
                    rtn = KeyCode.Joystick1Button10;
                break;
            case ControllerButtons.DPadUp:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.None;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button5;
                else
                    rtn = KeyCode.Joystick1Button13;
                break;
            case ControllerButtons.DPadDown:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.None;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button6;
                else
                    rtn = KeyCode.Joystick1Button14;
                break;
            case ControllerButtons.DPadLeft:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.None;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
                    rtn = KeyCode.Joystick1Button7;
                else
                    rtn = KeyCode.Joystick1Button11;
                break;
            case ControllerButtons.DPadRight:
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                    rtn = KeyCode.None;
                else if (Application.platform == RuntimePlatform.OSXPlayer)
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

    #endregion
}
using UnityEngine;

public class Button
{
    public enum ButtonType
    {
        Button,
        Axies
    }

    public string name;
    public KeyCode positive;
    public KeyCode negative;
    public ButtonType type = ButtonType.Button;

    public Button(string name, KeyCode key)
    {
        this.name = name;
        this.positive = key;
    }

    public Button(string name, KeyCode positive, KeyCode negative)
    {
        this.name = name;
        this.positive = positive;
        this.negative = negative;
    }
}
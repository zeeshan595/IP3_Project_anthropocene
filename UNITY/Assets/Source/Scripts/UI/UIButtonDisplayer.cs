using UnityEngine;

public class UIButtonDisplayer : MonoBehaviour
{
    [SerializeField]
    private bool isController = false;

    private void Start()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            if (!isController)
                gameObject.SetActive(false);
        }
        else
        {
            if (isController)
                gameObject.SetActive(false);
        }
    }
}
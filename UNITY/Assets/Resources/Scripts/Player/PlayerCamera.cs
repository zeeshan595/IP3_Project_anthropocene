using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float maxViewRange = 20;
    [SerializeField]
    private float lookOffset = 5.0f;
    [SerializeField]
    private bool inverted = false;
    [SerializeField]
    private Transform playerTarget;

    private Vector3 offset;
    private float rightVertical;

    // Use this for initialization
    void Start ()
    {
        offset = playerTarget.transform.position - transform.position;
        transform.SetParent(null);
        //Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        float angle = playerTarget.eulerAngles.y;
        
        if (!inverted)
            rightVertical += InputManager.GetAxies(ControllerAxies.RightStickY) + InputManager.GetAxies(ControllerAxies.MouseY);
        else
            rightVertical -= InputManager.GetAxies(ControllerAxies.RightStickY) - InputManager.GetAxies(ControllerAxies.MouseY);

        rightVertical = Mathf.Clamp(rightVertical, -maxViewRange, maxViewRange);
        Quaternion rotation = Quaternion.Euler(-rightVertical, angle, 0);
        transform.position = playerTarget.position - (rotation * offset);
        transform.LookAt(playerTarget.position + (playerTarget.forward * lookOffset));
	}
}

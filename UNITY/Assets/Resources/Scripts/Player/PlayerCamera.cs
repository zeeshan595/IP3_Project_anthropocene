using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public float rotateSpeed = 5;
    public float maxViewRange = 20;
    public float lookOffset = 5.0f;
    public bool inverted = false;
    public GameObject playerTarget;

    private Vector3 offset;
    private float rightVertical;

    // Use this for initialization
    void Start ()
    {
        offset = playerTarget.transform.position - transform.position;
        //Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
       // float rightVertical = InputManager.GetAxies(ControllerAxies.RightStickY) * rotateSpeed;
        float angle = playerTarget.transform.eulerAngles.y;
        
        if (!inverted)
            rightVertical += InputManager.GetAxies(ControllerAxies.RightStickY);
        else
            rightVertical -= InputManager.GetAxies(ControllerAxies.RightStickY);

        rightVertical = Mathf.Clamp(rightVertical, -maxViewRange, maxViewRange);
        Quaternion rotation = Quaternion.Euler(-rightVertical, angle, 0);
        transform.position = playerTarget.transform.position - (rotation * offset);
        transform.LookAt(playerTarget.transform.position + (playerTarget.transform.forward * lookOffset));
	}
}

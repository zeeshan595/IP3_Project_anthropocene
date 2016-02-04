using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public GameObject playerTarget;
    public GameObject aimTarget;
    public float rotateSpeed = 500;
    Vector3 offset;
    float rightVertical;
    public float maxViewRange = 20;
	// Use this for initialization
	void Start () {
        offset = playerTarget.transform.position - transform.position;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
       // float rightVertical = InputManager.GetAxies(ControllerAxies.RightStickY) * rotateSpeed;
        float angle = playerTarget.transform.eulerAngles.y;
        
        rightVertical -= InputManager.GetAxies(ControllerAxies.RightStickY);
        aimTarget.transform.Rotate(rightVertical * rotateSpeed *20* Time.deltaTime, 0, 0);
        rightVertical = Mathf.Clamp(rightVertical, -maxViewRange, maxViewRange);
        Quaternion rotation = Quaternion.Euler(-rightVertical, angle, 0);
        transform.position = playerTarget.transform.position - (rotation * offset);
        transform.LookAt(playerTarget.transform);
	}
}

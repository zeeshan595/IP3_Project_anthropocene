using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public GameObject playerTarget;
    public float rotateSpeed = 5;
    Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = playerTarget.transform.position - transform.position;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        float horizontal = InputManager.GetAxies(ControllerAxies.RightStickX) * rotateSpeed;
        playerTarget.transform.Rotate(0, horizontal, 0);

        float angle = playerTarget.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = playerTarget.transform.position - (rotation * offset);
         
        transform.LookAt(playerTarget.transform);
	}
}

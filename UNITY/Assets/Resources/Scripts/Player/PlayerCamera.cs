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
    private GameObject playerTarget;

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

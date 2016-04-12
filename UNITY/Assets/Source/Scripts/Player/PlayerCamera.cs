using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float minViewRange = 3;
    [SerializeField]
    private float maxViewRange = 5;
    [SerializeField]
    private float distance = 10;
    [SerializeField]
    private bool inverted = false;
    [SerializeField]
    private Transform playerTarget;
    [SerializeField]
    private float lookOffset = 1;
    [SerializeField]
    private float speed = 5.0f;

    private float rightVertical;

    private void Start ()
    {
        transform.SetParent(null);
        //Cursor.lockState = CursorLockMode.Locked;
	}
	
	private void Update () 
    {        
        if (!inverted)
            rightVertical += (InputManager.GetAxies(ControllerAxies.RightStickY) - InputManager.GetAxies(ControllerAxies.MouseY)) * Time.deltaTime * speed;
        else
            rightVertical -= (InputManager.GetAxies(ControllerAxies.RightStickY) - InputManager.GetAxies(ControllerAxies.MouseY)) * Time.deltaTime * speed;

        ///Change Position
        rightVertical = Mathf.Clamp(rightVertical, -minViewRange, maxViewRange);
        Vector3 pos = playerTarget.position; // Set Camera To Player Position
        pos += (-playerTarget.forward * distance);// Move Camera Back
        pos += (Vector3.up * rightVertical);// Move Camera Up

        transform.position = pos;
        transform.LookAt(playerTarget.position + (Vector3.up * lookOffset));
    }

    private void LateUpdate()
    {
        //If Wall infront
        Ray ray = new Ray(playerTarget.position - (transform.forward * 0.5f), transform.position - playerTarget.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            transform.position = hit.point;
        }
    }
}
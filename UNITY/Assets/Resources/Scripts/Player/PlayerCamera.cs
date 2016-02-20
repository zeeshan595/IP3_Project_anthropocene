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
    [SerializeField]
    private Vector3 offset;
    private Vector3 originalOffSet;
    private float rightVertical;
    public bool isWallBanging = false;

    private Vector3 velocity = Vector3.zero;
    // Use this for initialization
    void Start ()
    {
        transform.SetParent(null);
        //Cursor.lockState = CursorLockMode.Locked;
        originalOffSet = offset;
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
        transform.position = playerTarget.transform.position + playerTarget.transform.TransformDirection(offset);
        Vector3 playerPos = playerTarget.position + (playerTarget.forward * lookOffset);
        transform.LookAt(playerPos);

        Vector3 vec = Vector3.zero;
        RaycastHit hit = new RaycastHit();
        if (Physics.Linecast(playerTarget.position, transform.position, out hit))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {
               transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        }
	}

  

}

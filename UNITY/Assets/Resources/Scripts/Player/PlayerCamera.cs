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
        //StartCoroutine(OffSetCamera());
        Vector3 dir = playerTarget.position - transform.position;
        if (Physics.Raycast(transform.position, dir, dir.magnitude - 1))
        {
            Debug.Log("Object hit!");
            offset.z = 0;
        }
        else
        {
            Debug.Log("Why am I being turned off?");
            offset.z = 10;
        }
        //if (isWallBanging)
        //{
        //    offset.z = 0;
        //}
        //else if(!isWallBanging)
        //{
            
        //    offset.z = originalOffSet.z;
        //}
	}


    //IEnumerator OffSetCamera()
    //{
        
    //    yield return new WaitForSeconds(1);
    //    if(!Physics.Raycast(transform.position, dir, dir.magnitude - 1))
    //    {
    //        offset.z = originalOffSet.z;
    //    }
    //}
}

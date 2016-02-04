using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float rotateSpeed = 5;

    private CharacterController controller;
    private PlayerStats player;

	// Use this for initialization
	void Start ()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Move
        float vertical = -InputManager.GetAxies(ControllerAxies.LeftStickY);
        float horizontal = InputManager.GetAxies(ControllerAxies.LeftStickX);

        Vector3 targetDirection = new Vector3(horizontal, 0, vertical);
        if(!controller.isGrounded)
        {
            targetDirection.y -= player.gravity;
        }
        targetDirection = transform.TransformDirection(targetDirection);
        targetDirection *= player.speed;
        targetDirection *= Time.deltaTime;

        controller.Move(targetDirection);

        //Rotate
        float rightHorizontal = InputManager.GetAxies(ControllerAxies.RightStickX);
        transform.Rotate(0, rightHorizontal * rotateSpeed * 20 * Time.deltaTime, 0);
	}
}

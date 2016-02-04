using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    CharacterController controller;
    PlayerStats player;
    Vector3 offset;
    public float rotateSpeed = 500;
	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Move
        float vertical = InputManager.GetAxies(ControllerAxies.LeftStickY);
        float horizontal = InputManager.GetAxies(ControllerAxies.LeftStickX);
        Vector3 targetDirection = horizontal * transform.right + vertical * -transform.forward;
        controller.Move(targetDirection * player.speed * Time.deltaTime);
        //Rotate
        float rightHorizontal = InputManager.GetAxies(ControllerAxies.RightStickX);
        transform.Rotate(0, rightHorizontal * rotateSpeed *20* Time.deltaTime, 0);

	}
}

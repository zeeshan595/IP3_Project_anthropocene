using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float rotateSpeed = 5;
    public float jumpHeight = 20;
    public float jumpSpeed = 1;

    private CharacterController controller;
    private PlayerStats player;
    private bool isJumping = false;

	// Use this for initialization
	void Start ()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
    void Update()
    {
        //Move
        float vertical = -InputManager.GetAxies(ControllerAxies.LeftStickY);
        float horizontal = InputManager.GetAxies(ControllerAxies.LeftStickX);

        Vector3 targetDirection = new Vector3(horizontal, 0, vertical);
        if (!controller.isGrounded && !isJumping)
        {
            targetDirection.y -= player.gravity;
        }
        targetDirection = transform.TransformDirection(targetDirection);
        targetDirection *= player.speed;
        targetDirection *= Time.deltaTime;

        //Rotate
        float rightHorizontal = InputManager.GetAxies(ControllerAxies.RightStickX);
        transform.Rotate(0, rightHorizontal * rotateSpeed * 20 * Time.deltaTime, 0);

        //Jump
        if (InputManager.GetButton(ControllerButtons.A) && controller.isGrounded && !isJumping)
        {
            isJumping = true;
        }
        if (isJumping)
        {
            targetDirection.y = Mathf.Lerp(targetDirection.y, jumpHeight, jumpSpeed * Time.deltaTime);
            if (Mathf.Abs(jumpHeight - transform.position.y) < 0.5f)
            {
                isJumping = false;
            }
        }

        controller.Move(targetDirection);
    }
}

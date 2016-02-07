using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 5;
    [SerializeField]
    private float jumpHeight = 20;
    [SerializeField]
    private float jumpSpeed = 1;

    private CharacterController controller;
    private PlayerStats player;
    private bool isJumping = false;
    private float groundPos = 0;

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
        targetDirection *= player.playerSpeed;
        targetDirection *= Time.deltaTime;

        //Rotate
        float rightHorizontal = InputManager.GetAxies(ControllerAxies.RightStickX);
        transform.Rotate(0, rightHorizontal * rotateSpeed * 20 * Time.deltaTime, 0);

        //Jump
        if (InputManager.GetButton(ControllerButtons.A) && controller.isGrounded && !isJumping)
        {
            groundPos = transform.position.y;
            isJumping = true;
        }
        if (isJumping)
        {
            targetDirection.y = Mathf.Lerp(targetDirection.y, jumpHeight, jumpSpeed * Time.deltaTime);
            if (Mathf.Abs((jumpHeight + groundPos) - transform.position.y) < 0.5f)
            {
                isJumping = false;
            }
        }

        controller.Move(targetDirection);
    }
}

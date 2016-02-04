using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 10;
    CharacterController controller;
    PlayerStats player;
    Vector3 offset;
	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.W))
            controller.Move(transform.forward * Time.deltaTime * movementSpeed);
        if (Input.GetKey(KeyCode.D)) 
            controller.Move(transform.right * movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            controller.Move(-transform.right * movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            controller.Move(-transform.forward * Time.deltaTime * movementSpeed);
	}
}

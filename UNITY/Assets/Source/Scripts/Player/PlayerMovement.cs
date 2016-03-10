using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings(channel = 1, sendInterval = 0.1f)]
public class PlayerMovement : NetworkBehaviour
{
    #region Serialized Fields

    [SerializeField]
    public float rotateSpeed = 5;
    [SerializeField]
    public float jumpHeight = 20;
    [SerializeField]
    public float jumpSpeed = 1;
    //Network
    [SerializeField]
    private float transmitThreshold = 0.5f;
    [SerializeField]
    private float angularThreshHold = 1.0f;
    [SerializeField]
    private bool useTimeTravel = true;
    [SerializeField]
    private float normalLerpRate = 18.0f;
    [SerializeField]
    private float fastLerpRate = 25.0f;
    [SerializeField]
    private float angularLerpRate = 50.0f;

    #endregion

    #region Network Fields

    [SyncVar(hook = "OnSyncedPosition")]
    private Vector3 syncedPosition = Vector3.zero;
    [SyncVar(hook = "OnSyncedRotation")]
    private float syncedRotation = 0;

    #endregion

    #region Private Fields

    private CharacterController controller;
    private PlayerStats player;
    private bool isJumping = false;
    private float groundPos = 0;
    //Network
    private List<Vector3> syncedPositionList = new List<Vector3>();
    private List<float> syncedRotationList = new List<float>();
    private Vector3 lastPos = Vector3.zero;
    private float lastRot = 0;
    private float lerpRate;

    #endregion

    #region Private Methods

    private void Start ()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<PlayerStats>();
        lerpRate = normalLerpRate;
	}

    private void Update()
    {
        if (isLocalPlayer)//Local Player
        {
            //Move
            float vertical = -InputManager.GetAxies(ControllerAxies.LeftStickY);
            float horizontal = InputManager.GetAxies(ControllerAxies.LeftStickX);

            if (vertical == 0)
                vertical = InputManager.ButtonToAxies(KeyCode.W, KeyCode.S);

            if (horizontal == 0)
                horizontal = InputManager.ButtonToAxies(KeyCode.D, KeyCode.A);

            Vector3 targetDirection = new Vector3(horizontal, 0, vertical);
            if (!controller.isGrounded && !isJumping)
            {
                targetDirection.y -= player.gravity;
            }
            targetDirection = transform.TransformDirection(targetDirection);
            targetDirection *= player.playerSpeed;
            targetDirection *= Time.deltaTime;

            //Rotate
            float rightHorizontal = InputManager.GetAxies(ControllerAxies.RightStickX) + InputManager.GetAxies(ControllerAxies.MouseX);
            transform.Rotate(0, rightHorizontal * rotateSpeed * 20 * Time.deltaTime, 0);

            //Jump
            if ((Input.GetKey(KeyCode.Space) || InputManager.GetButton(ControllerButtons.A)) && controller.isGrounded && !isJumping)
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
        else//Network Sync
        {
            if (!useTimeTravel)
            {
                transform.position = Vector3.Lerp(transform.position, syncedPosition, Time.deltaTime * lerpRate);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, syncedRotation, 0)), Time.deltaTime * angularLerpRate);
            }
            else
            {
                //Position
                if (syncedPositionList.Count > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, syncedPositionList[0], Time.deltaTime * lerpRate);
                    if(Vector3.Distance(transform.position, syncedPositionList[0]) < 0.1f)
                    {
                        syncedPositionList.RemoveAt(0);
                    }
                    if (syncedPositionList.Count > 10)
                    {
                        lerpRate = fastLerpRate;
                    }
                    else
                    {
                        lerpRate = normalLerpRate;
                    }
                }

                //Rotation
                if (syncedRotationList.Count > 0)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, syncedRotationList[0], 0)), Time.deltaTime * angularLerpRate);
                    if (Mathf.Abs(syncedRotationList[0] - transform.rotation.eulerAngles.y) < 0.5f)
                    {
                        syncedRotationList.RemoveAt(0);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        TransmitInfo();
    }

    #endregion

    #region Server Commands

    [Command]
    private void CmdSendPositionToServer(Vector3 pos)
    {
        syncedPosition = pos;
    }

    [Command]
    private void CmdSendRotationToServer(float rot)
    {
        syncedRotation = rot;
    }

    #endregion

    #region Client Only Methods

    [ClientCallback]
    private void TransmitInfo()
    {
        if (isLocalPlayer)
        {
            if(Vector3.Distance(transform.position, lastPos) > transmitThreshold)
            {
                CmdSendPositionToServer(transform.position);
                lastPos = transform.position;
            }

            if(Mathf.Abs(lastRot - transform.rotation.eulerAngles.y) > angularThreshHold)
            {
                CmdSendRotationToServer(transform.rotation.eulerAngles.y);
                lastRot = transform.rotation.eulerAngles.y;
            }
        }
    }

    [ClientCallback]
    private void OnSyncedPosition(Vector3 pos)
    {
        syncedPosition = pos;
        syncedPositionList.Add(pos);
    }

    [ClientCallback]
    private void OnSyncedRotation(float rot)
    {
        syncedRotation = rot;
        syncedRotationList.Add(rot);
    }

    #endregion
}

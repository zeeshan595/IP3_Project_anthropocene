using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class PlayerNetwork : NetworkBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private bool useTimeTravel = false;
    [SerializeField]
    private float normalLerpRate = 18.0f;
    [SerializeField]
    private float fastLerpRate = 25.0f;
    [SerializeField]
    private float angularLerpRate;
    [SerializeField]
    private GameObject playerCamera;

    #endregion

    #region Network Variables

    [SyncVar (hook ="OnSyncedPosition")]
    private Vector3 syncedPosition = Vector3.zero;
    [SyncVar(hook = "OnSyncedRotation")]
    private Vector2 syncedRotation = Vector2.zero;
    [SyncVar]
    private NetworkInstanceId playerIdentity;

    #endregion

    #region Non Serialized Fields

    private List<Vector3> syncedPositionList = new List<Vector3>();
    private List<Vector2> syncedRotationList = new List<Vector2>();
    private Vector3 lastPos = Vector3.zero;
    private Vector2 lastRot = Vector2.zero;
    private float lerpRate;

    #endregion

    #region Private Methods

    private void Start()
    {
        lerpRate = normalLerpRate;
        if (!isLocalPlayer)
        {
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerWeapon>().enabled = false;
            Destroy(playerCamera);
        }
        else
        {
            GetNetIdentity();
        }
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            if (!useTimeTravel)
            {
                transform.position = Vector3.Lerp(transform.position, syncedPosition, Time.deltaTime * lerpRate);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, syncedRotation.y, 0), Time.deltaTime * angularLerpRate);
            }
            else
            {
                //Position
                if (syncedPositionList.Count > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, syncedPositionList[0], Time.deltaTime * lerpRate);
                    if (Vector3.Distance(transform.position, syncedPositionList[0]) < 0.1f)
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
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, syncedRotationList[0].y, 0), Time.deltaTime * angularLerpRate);
                    //TODO: head movment syncedRotationList[0].x
                    Vector2 newRot = new Vector2(0, transform.rotation.eulerAngles.y);
                    if (Vector2.Distance(newRot, syncedRotationList[0]) < 1)
                    {
                        syncedRotationList.RemoveAt(0);
                    }
                }
            }

            //Change player identity
            if (gameObject.name == "" || gameObject.name == "Player(Clone)")
            {
                gameObject.name = "Player " + playerIdentity.ToString();
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
    private void CmdSendRotationToServer(Vector2 rot)
    {
        syncedRotation = rot;
    }

    [Command]
    private void CmdGetPlayerIdentity(NetworkInstanceId identity)
    {
        playerIdentity = identity;
    }

    #endregion

    #region Client Methods

    [ClientCallback]
    private void TransmitInfo()
    {
        if (isLocalPlayer)
        {
            if (Vector3.Distance(transform.position, lastPos) > 0.5f)
            {
                CmdSendPositionToServer(transform.position);
                lastPos = transform.position;
            }

            //TODO: head movment syncedRotationList[0].x
            Vector2 newRot = new Vector2(0, transform.rotation.eulerAngles.y);
            if (Vector2.Distance(newRot, lastRot) > 0.5f)
            {
                CmdSendRotationToServer(newRot);
                lastRot = newRot;
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
    private void OnSyncedRotation(Vector2 rot)
    {
        syncedRotation = rot;
        syncedRotationList.Add(rot);
    }

    [ClientCallback]
    private void GetNetIdentity()
    {
        playerIdentity = GetComponent<NetworkIdentity>().netId;
        CmdGetPlayerIdentity(playerIdentity);
        gameObject.name = "Player " + playerIdentity.ToString();
    }

    #endregion
}
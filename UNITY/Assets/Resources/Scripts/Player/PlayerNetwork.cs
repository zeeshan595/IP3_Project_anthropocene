using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject playerNameText;

    [SyncVar]
    private string playerIdentity;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            playerCamera.GetComponent<PlayerCamera>().enabled = false;
            Destroy(playerCamera);
        }
        else
        {
            gameObject.name = "Player " + GetPlayerIdentity();
            SendPlayerIdentity();
        }
    }

    private void Update()
    {
        if (gameObject.name == "" || gameObject.name == "Player(Clone)")
        {
            gameObject.name = playerIdentity;
        }
    }

    private string GetPlayerIdentity()
    {
        playerIdentity = "Player " + GetComponent<NetworkIdentity>().netId;
        return playerIdentity;
    }

    [Command]
    private void CmdSetPlayerIdentity(string identity)
    {
        playerIdentity = identity;
    }

    [ClientCallback]
    private void SendPlayerIdentity()
    {
        CmdSetPlayerIdentity(playerIdentity);
    }
}
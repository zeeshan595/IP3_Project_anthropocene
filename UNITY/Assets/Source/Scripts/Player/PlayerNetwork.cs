using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class PlayerNetwork : NetworkBehaviour
{
    #region Getters & Setters

    public string Username
    {
        get
        {
            return playerName;
        }
    }

    #endregion

    #region Serialized Fields

    [SerializeField]
    public GameObject playerCamera;
    [SerializeField]
    private GameObject playerNameText;

    #endregion

    #region Sync Variables

    [SyncVar(hook = "OnPlayerIdentityChanged")]
    private string playerIdentity;
    [SyncVar(hook = "OnPlayerNameChanged")]
    private string playerName = "";

    #endregion

    #region Private Variables

    private Transform camTransform;
    private PlayerStats player;

    #endregion

    #region Private Methods

    private void Start()
    {
        if (!isLocalPlayer)
        {
            playerCamera.GetComponent<PlayerCamera>().enabled = false;
            player = GetComponent<PlayerStats>();
            Destroy(playerCamera);
            camTransform = Camera.main.gameObject.transform;
            if (Settings.team != player.team)
            {
                playerNameText.SetActive(false);
            }
        }
        else
        {
            playerIdentity = "Player " + GetComponent<NetworkIdentity>().netId;
            gameObject.name = playerIdentity;
            playerName = Settings.username;
            SendPlayerIdentity();
            SendPlayerName();
            playerNameText.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (camTransform != null)
            playerNameText.transform.LookAt(camTransform.forward + playerNameText.transform.position);
    }

    private string GetPlayerIdentity()
    {
        
        return playerIdentity;
    }

    #endregion

    #region Hook

    private void OnPlayerIdentityChanged(string playerIdentity)
    {
        this.playerIdentity = playerIdentity;
    }

    private void OnPlayerNameChanged(string playerName)
    {
        this.playerName = playerName;
        playerNameText.GetComponent<TextMesh>().text = playerName;
        gameObject.name = playerName;
    }

    #endregion

    #region Client Call Back

    [ClientCallback]
    private void SendPlayerIdentity()
    {
        CmdSetPlayerIdentity(playerIdentity);
    }

    [ClientCallback]
    private void SendPlayerName()
    {
        CmdSetPlayername(Settings.username);
    }

    #endregion

    #region Command

    [Command]
    private void CmdSetPlayerIdentity(string identity)
    {
        playerIdentity = identity;
    }

    [Command]
    private void CmdSetPlayername(string username)
    {
        playerName = username;
    }

    #endregion
}
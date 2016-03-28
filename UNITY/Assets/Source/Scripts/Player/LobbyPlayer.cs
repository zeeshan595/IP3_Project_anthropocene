using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer
{
    [SyncVar]
    public string username;
    [SyncVar]
    public TeamType team = TeamType.Red;
    [SyncVar]
    private int playerAmount = 0;

    private LobbyManager manager;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (isLocalPlayer)
        {
            CmdChangeUsername(Settings.username);
        }
        manager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
    }

    private void Update()
    {
        manager.connectedPlayers = playerAmount;
        if (isServer)
        {
            playerAmount = manager.numPlayers;
        }
    }

    public void Ready()
    {
        if (isLocalPlayer)
        {
            readyToBegin = true;
            SendReadyToBeginMessage();
        }
    }

    public void UnReady()
    {
        if (isLocalPlayer)
        {
            readyToBegin = false;
            SendNotReadyToBeginMessage();
        }
    }

    [ClientCallback]
    public void ChangeTeam(TeamType team)
    {
        if (isLocalPlayer)
        {
            Settings.team = team;
            CmdChangeTeam(team);
        }
        else
        {
            Debug.Log("You cannot change other player's team");
        }
    }

    [Command]
    private void CmdChangeTeam(TeamType team)
    {
        this.team = team;
    }

    [Command]
    private void CmdChangeUsername(string user)
    {
        username = user;
    }
}
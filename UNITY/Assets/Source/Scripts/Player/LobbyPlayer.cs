using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer
{
    [SyncVar]
    public string username;
    [SyncVar]
    public TeamType team = TeamType.Blue;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (isLocalPlayer)
        {
            username = Settings.username;
            CmdChangeUsername(Settings.username);
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
    private void CmdChangeUsername(string username)
    {
        this.username = username;
    }
}
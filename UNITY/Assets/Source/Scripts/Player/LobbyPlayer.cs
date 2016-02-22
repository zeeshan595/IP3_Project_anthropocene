using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer
{
    [SyncVar]
    public TeamType team = TeamType.Blue;

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
}
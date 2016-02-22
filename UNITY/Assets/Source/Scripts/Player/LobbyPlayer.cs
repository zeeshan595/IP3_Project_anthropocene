using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer
{
    [SyncVar]
    public TeamType team = TeamType.Blue;

    public void Ready()
    {
        readyToBegin = true;
        SendReadyToBeginMessage();
    }

    public void UnReady()
    {
        readyToBegin = false;
        SendNotReadyToBeginMessage();
    }
}
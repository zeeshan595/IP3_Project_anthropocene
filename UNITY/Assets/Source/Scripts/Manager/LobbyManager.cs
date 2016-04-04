using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using UnityEngine.Networking.Types;

public delegate void OnConnectedToServer(NetworkConnection conn);
public delegate void OnConntionLostFromServer(NetworkConnection conn);
public delegate void OnClientConnectedToServer(NetworkConnection conn);

public class LobbyManager : NetworkLobbyManager
{
    public string version = "Alpha0.02";

    //Event Handlers
    public OnConnectedToServer ClientConnected = null;
    public OnConntionLostFromServer ClientDisconnected = null;
    public int connectedPlayers = 0;

    private bool isSearchingForMatch = false;
    private bool isMatchFound = false;
    private long networkID;
    private long nodeID;

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if (ClientConnected != null)
            ClientConnected(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        if (ClientDisconnected != null)
            ClientDisconnected(conn);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        Character ch = Character.Potatree;
        Transform spawnPosition = transform;
        for (int i = 0; i < connectedPlayers; i++)
        {
            LobbyPlayer player = lobbySlots[i].GetComponent<LobbyPlayer>();
            if (player.connectionToClient == conn)
            {
                if (player)
                {
                    ch = player.character;
                    if (player.team == TeamType.Blue)
                    {
                        spawnPosition.position = new Vector3(10, 8, 10);
                        spawnPosition.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                    }
                    else
                    {
                        spawnPosition.position = new Vector3(-70, 8, 10);
                        spawnPosition.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    }
                    break;
                }
                break;
            }
        }

        GameObject obj = (GameObject)Instantiate(gamePlayerPrefab, spawnPosition.position, spawnPosition.rotation);
        obj.GetComponent<PlayerStats>().character = ch;
        return obj;
    }

    private void Update()
    {
        minPlayers = connectedPlayers;
    }
}
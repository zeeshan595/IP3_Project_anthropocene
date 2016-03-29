using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public delegate void OnConnectedToServer(NetworkConnection conn);
public delegate void OnConntionLostFromServer(NetworkConnection conn);
public delegate void OnClientConnectedToServer(NetworkConnection conn);

public class LobbyManager : NetworkLobbyManager
{
    public string version = "Alpha0.01";

    //Event Handlers
    public OnConnectedToServer ClientConnected = null;
    public OnConntionLostFromServer ClientDisconnected = null;
    public int connectedPlayers = 0;

    private bool isSearchingForMatch = false;
    private bool isMatchFound = false;

    #region Public Methods

    public static void Connect(string ip, int port)
    {
        LobbyManager.singleton.networkAddress = ip;
        LobbyManager.singleton.networkPort = port;
        LobbyManager.singleton.StartClient();
    }

    public void CreateHost()
    {
        StartMatchMaker();
        CreateMatchRequest request = new CreateMatchRequest();
        request.name = version;
        request.size = (uint)maxConnections;
        request.advertise = true;
        request.password = string.Empty;

        matchMaker.CreateMatch(request, OnMatchCreate);
    }

    public void SearchForMatch()
    {
        StartMatchMaker();
        isSearchingForMatch = true;
        matchMaker.ListMatches(0, 50, string.Empty, MatchList);
    }

    public void StopSearchForMatch()
    {
        StopMatchMaker();
        isSearchingForMatch = false;
    }

    #endregion

    #region Private Methods

    private void MatchList(ListMatchResponse matchList)
    {
        if (!isSearchingForMatch)
            return;

        matches = matchList.matches;
        isMatchFound = false;
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].currentSize < matches[i].maxSize && matches[i].name == version)
            {
                isMatchFound = true;
                matchMaker.JoinMatch(matches[i].networkId, string.Empty, MatchJoined);
                Debug.Log("Match Found: " + matches[i].name + "|" + matches[i].currentSize);
                break;
            }
        }
        if (!isMatchFound)
        {
            //matchMaker.ListMatches(0, 100, string.Empty, MatchList);
            StartCoroutine(waitBeforeFind());
        }
    }

    private IEnumerator waitBeforeFind()
    {
        yield return new WaitForSeconds(1);
        matchMaker.ListMatches(0, 100, string.Empty, MatchList);
    }

    private void MatchJoined(JoinMatchResponse matchInfo)
    {
        if (matchInfo.success)
        {
            StartClient(new MatchInfo(matchInfo));
        }
        else
        {
            matchMaker.ListMatches(0, 100, string.Empty, MatchList);
            isMatchFound = false;
        }
    }

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

    public override void OnMatchCreate(CreateMatchResponse matchInfo)
    {
        if (matchInfo.success)
        {
            StartHost(new MatchInfo(matchInfo));
        }
    }

    private void Update()
    {
        minPlayers = connectedPlayers;
    }

    #endregion
}
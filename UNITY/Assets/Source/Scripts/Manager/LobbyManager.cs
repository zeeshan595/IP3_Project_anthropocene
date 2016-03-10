using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public delegate void OnConnectedToServer(NetworkConnection conn);
public delegate void OnConntionLostFromServer(NetworkConnection conn);

public class LobbyManager : NetworkLobbyManager
{
    //Event Handlers
    public OnConnectedToServer ClientConnected = null;
    public OnConntionLostFromServer ClientDisconnected = null;

    private bool isSearchingForMatch = false;
    private bool isMatchFound = false;

    #region Public Methods

    public void CreateHost()
    {
        CreateMatchRequest request = new CreateMatchRequest();
        request.name = "";
        request.size = (uint)maxConnections;
        request.advertise = true;
        request.password = string.Empty;

        matchMaker.CreateMatch(request, OnMatchCreate);
    }

    public void SearchForMatch()
    {
        isSearchingForMatch = true;
        matchMaker.ListMatches(0, 50, string.Empty, MatchList);
    }

    public void StopSearchForMatch()
    {
        isSearchingForMatch = false;
    }

    #endregion

    #region Private Methods

    private void Start()
    {
        StartMatchMaker();
    }

    private void MatchList(ListMatchResponse matchList)
    {
        if (!isSearchingForMatch)
            return;

        matches = matchList.matches;
        isMatchFound = false;
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].currentSize < matches[i].maxSize)
            {
                isMatchFound = true;
                matchMaker.JoinMatch(matches[i].networkId, string.Empty, MatchJoined);
                Debug.Log("Match Found");
                break;
            }
        }
        if (!isMatchFound)
        {
            matchMaker.ListMatches(0, 100, string.Empty, MatchList);
        }
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
        Transform spawnPosition = transform;
        for (int i = 0; i < lobbySlots.Length; i++)
        {
            LobbyPlayer player = lobbySlots[i].GetComponent<LobbyPlayer>();
            if (player.playerControllerId == playerControllerId)
            {
                if (player)
                {
                    if (player.team == TeamType.Blue)
                    {
                        spawnPosition.position = new Vector3(10, 8, 10);
                        spawnPosition.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                    }
                    else
                    {
                        spawnPosition.position = new Vector3(-70, 8, 4);
                        spawnPosition.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    }
                    break;
                }
                break;
            }
        }

        return (GameObject)Instantiate(gamePlayerPrefab, spawnPosition.position, spawnPosition.rotation);
    }

    public override void OnMatchCreate(CreateMatchResponse matchInfo)
    {
        if (matchInfo.success)
        {
            singleton.StartHost(new MatchInfo(matchInfo));
        }
    }

    #endregion
}
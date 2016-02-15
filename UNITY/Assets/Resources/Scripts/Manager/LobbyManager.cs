using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public delegate void OnConnectedToServer(NetworkConnection conn);
public delegate void OnConntionLostFromServer(NetworkConnection conn);

public class LobbyManager : NetworkLobbyManager
{
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

    #endregion
}
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] animationSprites = new GameObject[3];
    [SerializeField]
    private GameObject searchingForMatchScreen;
    [SerializeField]
    private GameObject matchFoundScreen;
    [SerializeField]
    private GameObject[] redTeam;
    [SerializeField]
    private GameObject[] blueTeam;

    private Dictionary<int, int> redTeamPlayers = new Dictionary<int, int>();
    private Dictionary<int, int> blueTeamPlayers = new Dictionary<int, int>();
    private LobbyManager manager;
    private bool isConnected = false;
    private bool isAnimationPlaying = false;

    private IEnumerator Start()
    {
        FindLobbyManager();

        yield return manager;

        manager.ClientConnected += OnConnected;
        manager.ClientDisconnected += OnDisconnect;
        StartCoroutine(FindAGame());
    }

    private void Update()
    {
        if (!isConnected && !isAnimationPlaying)
        {
            StartCoroutine(loadingAnimation());
        }

        UpdateTeams();

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeTeam(TeamType.Red);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeTeam(TeamType.Blue);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReadyPlayer(true);
        }
    }

    #region Network Stuff

    private IEnumerator FindAGame()
    {
        manager.SearchForMatch();

        yield return new WaitForSeconds(3);

        manager.StopSearchForMatch();
        manager.CreateHost();
    }

    private void FindLobbyManager()
    {
        GameObject obj = GameObject.Find("LobbyManager");
        if (obj != null)
            manager = obj.GetComponent<LobbyManager>();
        else
            Debug.LogWarning("Couldn't Find Lobby Manager");

        if (manager == null)
        {
            Debug.LogError("<h2>Couldn't Locate Lobby Manager Component</h2>");
            FindLobbyManager();
        }
    }

    private void OnConnected(NetworkConnection conn)
    {
        searchingForMatchScreen.SetActive(false);
        matchFoundScreen.SetActive(true);
        isConnected = true;
    }

    private void OnDisconnect(NetworkConnection conn)
    {
        searchingForMatchScreen.SetActive(false);
        matchFoundScreen.SetActive(true);
        isConnected = false;
        StartCoroutine(FindAGame());
        Debug.LogWarning("Client Was Disconnected. Attempting to Reconnect To A Match");
    }

    #endregion

    #region Update UI

    private void UpdateTeams()
    {
        for (int i = 0; i < manager.lobbySlots.Length; i++)
        {
            if (manager.lobbySlots[i])
            {
                LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
                if (!redTeamPlayers.ContainsKey(i) && player.team == TeamType.Red)
                {
                    //Tie red team with that player
                    redTeamPlayers.Add(i, redTeamPlayers.Count);
                    ChangePlayerName(redTeam[redTeamPlayers[i]], player.username);
                }
                else if (!blueTeamPlayers.ContainsKey(i) && player.team == TeamType.Blue)
                {
                    //Tie blue team with that player
                    blueTeamPlayers.Add(i, blueTeamPlayers.Count);
                    ChangePlayerName(blueTeam[blueTeamPlayers[i]], player.username);
                }
            }
        }
    }

    private void ChangeTeam(TeamType team)
    {
        for (int i = 0; i < manager.lobbySlots.Length; i++)
        {
            if (manager.lobbySlots[i])
            {
                LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
                if (manager.lobbySlots[i])
                {
                    if (Settings.username == player.username)
                    {
                        if (player.team == TeamType.Red)
                        {
                            ChangePlayerName(redTeam[redTeamPlayers[i]], "Waiting for user");
                            redTeamPlayers.Remove(i);
                        }
                        else
                        {
                            ChangePlayerName(blueTeam[blueTeamPlayers[i]], "Waiting for user");
                            blueTeamPlayers.Remove(i);
                        }
                        player.team = team;
                    }
                }
            }
        }
    }

    private void ReadyPlayer(bool ready)
    {
        for (int i = 0; i < manager.lobbySlots.Length; i++)
        {
            if (manager.lobbySlots[i])
            {
                LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
                if (manager.lobbySlots[i])
                {
                    if (Settings.username == player.username)
                    {
                        if (ready)
                            player.Ready();
                        else
                            player.UnReady();
                    }
                }
            }
        }
    }

    private void ChangePlayerName(GameObject obj, string username)
    {
        Text child = obj.transform.GetChild(0).GetComponent<Text>();
        child.text = username;
    }

    private IEnumerator loadingAnimation()
    {
        isAnimationPlaying = true;
        animationSprites[0].SetActive(true);
        yield return new WaitForSeconds(0.35f);
        animationSprites[1].SetActive(true);
        yield return new WaitForSeconds(0.35f);
        animationSprites[2].SetActive(true);
        yield return new WaitForSeconds(0.35f);
        foreach (GameObject gj in animationSprites)
            gj.SetActive(false);
        yield return new WaitForSeconds(0.35f);
        isAnimationPlaying = false;
    }

    #endregion
}
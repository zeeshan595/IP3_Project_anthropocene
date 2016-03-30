using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

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

    private List<int> redTeamPlayers = new List<int>();
    private List<int> blueTeamPlayers = new List<int>();
    private LobbyManager manager;
    private bool isConnected = false;
    private bool isAnimationPlaying = false;

    private IEnumerator Start()
    {
        Settings.username = "user " + Random.Range(0, 99);
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

        CheckForNewUser();
        ChangeTeam();

        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < manager.connectedPlayers; i++)
            {
                if (manager.lobbySlots[i].GetComponent<LobbyPlayer>().isLocalPlayer)
                {
                    manager.lobbySlots[i].GetComponent<LobbyPlayer>().ChangeTeam(TeamType.Red);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < manager.connectedPlayers; i++)
            {
                if (manager.lobbySlots[i].GetComponent<LobbyPlayer>().isLocalPlayer)
                {
                    manager.lobbySlots[i].GetComponent<LobbyPlayer>().ChangeTeam(TeamType.Blue);
                }
            }
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
        Debug.Log("Starting client");

        yield return new WaitForSeconds(30);

        if (!isConnected)
        {
            manager.StopSearchForMatch();
            yield return new WaitForSeconds(1);
            manager.CreateHost();
            Debug.Log("Starting Host");
        }
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

    private void CheckForNewUser()
    {
        for (int i = 0; i < manager.connectedPlayers; i++)
        {
            if (manager.lobbySlots[i])
            {
                if (redTeamPlayers.IndexOf(i) == -1 && blueTeamPlayers.IndexOf(i) == -1)
                {
                    if (manager.lobbySlots[i].GetComponent<LobbyPlayer>().team == TeamType.Red)
                    {
                        redTeamPlayers.Add(i);
                    }
                    else
                    {
                        blueTeamPlayers.Add(i);
                    }
                }
            }
        }
    }

    private void ChangeTeam()
    {
        //Change team
        for (int i = 0; i < manager.connectedPlayers; i++)
        {
            if (manager.lobbySlots[i])
            {
                LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
                if (redTeamPlayers.IndexOf(i) != -1 && player.team == TeamType.Blue)
                {
                    redTeamPlayers.Remove(i);
                    blueTeamPlayers.Add(i);                    
                }
                else if (blueTeamPlayers.IndexOf(i) != -1 && player.team == TeamType.Red)
                {
                    blueTeamPlayers.Remove(i);
                    redTeamPlayers.Add(i);
                }
            }
        }

        //Reset All feilds
        for (int i = 0; i < redTeam.Length; i++)
        {
            ChangePlayerName(redTeam[i], null);
        }

        for (int i = 0; i < blueTeam.Length; i++)
        {
            ChangePlayerName(blueTeam[i], null);
        }

        //Put user names in correct position
        for (int i = 0; i < redTeamPlayers.Count; i++)
        {
            LobbyPlayer player = manager.lobbySlots[redTeamPlayers[i]].GetComponent<LobbyPlayer>();
            ChangePlayerName(redTeam[i], player);
        }

        for (int i = 0; i < blueTeamPlayers.Count; i++)
        {
            LobbyPlayer player = manager.lobbySlots[blueTeamPlayers[i]].GetComponent<LobbyPlayer>();
            ChangePlayerName(blueTeam[i], player);
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

    private void ChangePlayerName(GameObject obj, LobbyPlayer player)
    {
        Text child = obj.transform.FindChild("Text").GetComponent<Text>();
        if (player != null)
        {
            child.text = player.username;
            obj.transform.FindChild("Image").gameObject.SetActive(player.readyToBegin);
        }
        else
        {
            child.text = "Waiting For User...";
            obj.transform.FindChild("Image").gameObject.SetActive(false);
        }
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
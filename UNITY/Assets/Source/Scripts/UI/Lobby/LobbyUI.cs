using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using Battlehub.Dispatcher;

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
    [SerializeField]
    private GameObject connectionInfo;

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
        FindAGame();
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

    private void OnDisable()
    {
        MatchMakerClient.StopMatchMaker();
    }

    private void FindAGame()
    {
        Dispatcher.Current.BeginInvoke(() =>
        {
            connectionInfo.GetComponent<Text>().text += "Attempting to connect to Match maker server...\n";
        });
        MatchMakerClient.connectCallback = connectCallBack;
        MatchMakerClient.StartMatchMaker();
    }

    private void connectCallBack(System.Net.Sockets.Socket socket)
    {
        Dispatcher.Current.BeginInvoke(() =>
        {
            connectionInfo.GetComponent<Text>().text += "Connected\n";
            connectionInfo.GetComponent<Text>().text += "Getting rooms list\n";
        });
        MatchMakerClient.listRoomsCallback = listRoomCallBack;
        MatchMakerClient.ListRoom();
    }

    private void listRoomCallBack(List<MatchMakerPacket.Room> rooms)
    {
        Dispatcher.Current.BeginInvoke(() =>
        {
            connectionInfo.GetComponent<Text>().text += "Got responce.\n";
        });
        if (rooms.Count == 0)
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                connectionInfo.GetComponent<Text>().text += "No Rooms Found\n";
                connectionInfo.GetComponent<Text>().text += "Creating Room\n";
            });
            
            MatchMakerClient.CreateRoom("Alpha", "", 8);
            Host();
        }
        else
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                connectionInfo.GetComponent<Text>().text += "Attempting to search through rooms\n";
            });
            bool joined = false;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].currentPlayers < rooms[i].maxPlayers)
                {
                    Dispatcher.Current.BeginInvoke(() =>
                    {
                        manager.networkPort = 7777;
                        manager.networkAddress = rooms[i].hostIP;
                        manager.StartClient();
                    });
                }
            }

            if (!joined)
            {
                Dispatcher.Current.BeginInvoke(() =>
                {
                    connectionInfo.GetComponent<Text>().text += "No Rooms Found\n";
                    connectionInfo.GetComponent<Text>().text += "Creating Room\n";
                });
                MatchMakerClient.CreateRoom("Alpha", "", 8);
                Host();
            }
        }
    }

    private void Host()
    {
        Dispatcher.Current.BeginInvoke(() =>
        {
            manager.StartHost();
        });
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
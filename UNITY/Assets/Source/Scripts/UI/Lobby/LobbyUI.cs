using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyUI : MonoBehaviour
{
    private LobbyManager manager;
    private bool isMatchFound = false;

    //Loading Animation
    public GameObject[] animationSprites = new GameObject[3];
    private bool isAnimationPlaying = false;
    public GameObject searchingForMatchScreen;
    public GameObject matchFoundScreen;

    //Ready players
    public GameObject[] players = new GameObject[8]; 
    private List<Image> imageList = new List<Image>();

    //Change Teams
    private Dictionary<LobbyPlayer, GameObject> redDictionary = new Dictionary<LobbyPlayer,GameObject>();
    private Dictionary<LobbyPlayer, GameObject> blueDictionary = new Dictionary<LobbyPlayer, GameObject>();
    public GameObject[] redObjects = new GameObject[4];
    public GameObject[] blueObjects = new GameObject[4];

    private IEnumerator Start()
    {
        FindLobbyManager();
        Debug.Log(Settings.username);
        FindPlayerObjects();
        yield return new WaitForSeconds(0.5f);
        manager.SearchForMatch();
        manager.ClientConnected += WeFoundAMatch;
        Debug.Log(manager.IsClientConnected());
        yield return new WaitForSeconds(5.0f);
        manager.StopSearchForMatch();
        if (!isMatchFound)
        {
            manager.CreateHost();
            isMatchFound = true;
            ChangeScreen();
        }
        InvokeRepeating("UpdateTeams", 3.0f, 3.0f);
    }

    private void WeFoundAMatch(NetworkConnection conn)
    {
        isMatchFound = true;
        Debug.Log("Just testing");
    }

    private void Update()
    {
        if (!isMatchFound && !isAnimationPlaying)
        {
            StartCoroutine(loadingAnimation());
        }

        if (manager == null)
            FindLobbyManager();

        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangePlayerTeam(TeamType.Blue);
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            ChangePlayerTeam(TeamType.Red);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ReadyPlayer();
        }

    }

    #region Find Objects
    private void FindLobbyManager()
    {
        GameObject obj = GameObject.Find("LobbyManager");
        if (obj != null)
            manager = obj.GetComponent<LobbyManager>();
        else
            Debug.LogWarning("Couldn't Find Lobby Manager");

        if (manager == null)
        {
            Debug.LogError("<h2>I hate my life</h2>");
        }
    }

    private void FindPlayerObjects()
    {
        for(int i = 0; i < manager.lobbySlots.Length; i++)
        {
            imageList.Add(players[i].GetComponent<Image>());
            ChangePlayerName(players[i]);
        }
    }

    #endregion

    #region network updates

    private void ChangePlayerTeam(TeamType team)
    {
        for (int i = 0; i < manager.lobbySlots.Length; i++)
        {
            LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
            if (player.username == Settings.username)
            {
                player.ChangeTeam(team);
            }
        }
        UpdateTeams();
    }

    private void ReadyPlayer()
    {
        for (int i = 0; i < manager.lobbySlots.Length; i++)
        {
            LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
            if (player.username == Settings.username)
            {
                player.Ready();
            }
        }
        ReadyPlayerUI();
    }

    private void UpdateTeams()
    {
        for (int i = 0; i < manager.lobbySlots.Length; i++)
        {
            LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
            if (player.team == TeamType.Red)
            {
                if (!redDictionary.ContainsKey(player))
                {
                    redDictionary.Add(player, redObjects[i]);
                }
            }
            else if(player.team == TeamType.Blue)
            {
                if(!blueDictionary.ContainsKey(player))
                {
                    blueDictionary.Add(player, blueObjects[i]);
                }
            }
        }
        ChangePlayerTeamUI();
    }

    #endregion

    #region UI changes
    private void ChangePlayerTeamUI()
    {
        foreach(KeyValuePair<LobbyPlayer, GameObject> entry in redDictionary)
        {
            ChangePlayerName(entry.Value);
        }
    }

    private void ReadyPlayerUI()
    {
        for (int i = 0; i < manager.lobbySlots.Length; i++)
        {
            LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
            if(player.readyToBegin)
            {
                imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, 0.5f);
            }
        }
    }

    //Not yet tested, didnt have a lobby
    private void ChangePlayerName(GameObject playerObject)
    {
        Text child = playerObject.transform.GetChild(0).GetComponent<Text>();
        for (int j = 0; j < manager.lobbySlots.Length; j++)
        {
            LobbyPlayer player = manager.lobbySlots[j].GetComponent<LobbyPlayer>();
            Debug.Log(player.name);
            child.text = player.name;
        }
    }

    IEnumerator loadingAnimation()
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

    private void ChangeScreen()
    {
        searchingForMatchScreen.SetActive(false);
        matchFoundScreen.SetActive(true);
    }

    #endregion
}
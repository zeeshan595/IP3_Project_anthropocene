using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyUI : MonoBehaviour
{
    private LobbyManager manager;
    private bool isMatchFound = false;

    private IEnumerator Start()
    {
        FindLobbyManager();
        Debug.Log("<color=red>Muahaha</color>");
        yield return new WaitForSeconds(0.5f);
        manager.SearchForMatch();
        manager.ClientConnected += WeFoundAMatch;
        yield return new WaitForSeconds(5.0f);
        manager.StopSearchForMatch();
        if (isMatchFound)
        {
            manager.CreateHost();
            isMatchFound = true;
        }
    }

    private void WeFoundAMatch(NetworkConnection conn)
    {
        isMatchFound = true;
        Debug.Log("Just testing");
    }

    private void Update()
    {
        if (manager == null)
            FindLobbyManager();

        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < manager.lobbySlots.Length; i++)
            {
                LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
                if (player.username == Settings.username)
                {
                    player.ChangeTeam(TeamType.Red);
                    player.Ready();
                    
                }
            }
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
            Debug.LogError("<h2>I hate my life</h2>");
        }
    }
}
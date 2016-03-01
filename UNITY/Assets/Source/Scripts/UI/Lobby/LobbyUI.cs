﻿using System;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyUI : MonoBehaviour
{
    public GameObject offlineUI;
    public GameObject onlineUI;

    private LobbyManager lobby;
    private bool isRed = false;

    private void Start()
    {
        //Get Lobby manager at the start
        lobby = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        //Tell lobby manager what method to call when connected to server
        lobby.ClientConnected += new OnConnectedToServer(OnConnected);
        //Tell lobby manager what method to call when disconnected from server
        lobby.ClientDisconnected += new OnConntionLostFromServer(OnDisconnected);
        //Enable offline UI so user can connect to server
        offlineUI.SetActive(true);
        onlineUI.SetActive(false);
    }

    private void OnDisconnected(NetworkConnection conn)
    {
        //When disconnected from server enable offline UI
        offlineUI.SetActive(true);
        onlineUI.SetActive(false);
    }

    private void OnConnected(NetworkConnection conn)
    {
        //When connected enable online UI
        offlineUI.SetActive(false);
        onlineUI.SetActive(true);

        for (int i = 0; i < lobby.lobbySlots.Length; i++)
        {
            if (lobby.lobbySlots[i] != null)
            {
                LobbyPlayer player = lobby.lobbySlots[i].GetComponent<LobbyPlayer>();
                if (player != null)
                {
                    //When player joins what needs to happen

                    //Ready the player
                    //player.Ready();

                    //Unready the player
                    //player.UnReady();
                }
            }
        }
    }


    //Button Methods
    public void CreateMatch()
    {
        lobby.CreateHost();
    }

    public void FindMatch()
    {
        lobby.SearchForMatch();
    }

    //Simple username textbox
    private void OnGUI()
    {
        Settings.username = GUILayout.TextField(Settings.username, GUILayout.Width(200));
        isRed = GUILayout.Toggle(isRed, "Is Red");
        if (isRed)
        {
            Settings.team = TeamType.Red;
        }
        else
        {
            Settings.team = TeamType.Blue;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TopBar : MonoBehaviour
{
    public Sprite[] spriteRed;
    public Sprite[] spriteBlue;

    [SerializeField]
    private GameObject[] blueTeam;
    [SerializeField]
    private GameObject[] redTeam;

    private List<int> blueTeamPlayers = new List<int>();
    private List<int> redTeamPlayers = new List<int>();

    LobbyManager manager;

    private void Start()
    {
        manager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        for (int i = 0; i < manager.connectedPlayers; i++)
        {
            if (manager.lobbySlots[i].GetComponent<LobbyPlayer>().team == TeamType.Red)
            {
                redTeamPlayers.Add((int)manager.lobbySlots[i].GetComponent<LobbyPlayer>().character);
            }
            else
            {
                blueTeamPlayers.Add((int)manager.lobbySlots[i].GetComponent<LobbyPlayer>().character);
            }
        }
    }

    private void Update()
    {
        //Reset All feilds
        for (int i = 0; i < redTeam.Length; i++)
        {
            if (redTeamPlayers.Count > i)
            {
                redTeam[i].SetActive(true);
                LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
                redTeam[i].GetComponent<Image>().sprite = spriteRed[redTeamPlayers[i]];
            }
            else
            {
                redTeam[i].SetActive(false);
            }
        }

        for (int i = 0; i < blueTeam.Length; i++)
        {
            if (blueTeamPlayers.Count > i)
            {
                blueTeam[i].SetActive(true);
                LobbyPlayer player = manager.lobbySlots[i].GetComponent<LobbyPlayer>();
                blueTeam[i].GetComponent<Image>().sprite = spriteBlue[blueTeamPlayers[i]];
            }
            else
            {
                blueTeam[i].SetActive(false);
            }
        }
    }
}
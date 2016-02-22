using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField]
    private Transform[] spawnPositions;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, spawnPositions[Random.Range(0, spawnPositions.Length)].position, spawnPositions[Random.Range(0, spawnPositions.Length)].rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    private void Start()
    {
        StartupHost();
    }

    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
}

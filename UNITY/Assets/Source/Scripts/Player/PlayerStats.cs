using UnityEngine;
using UnityEngine.Networking;

public enum TeamType
{
    Red,
    Blue
}

public class PlayerStats : NetworkBehaviour
{
    [SyncVar]
    public TeamType team;
    [SyncVar]
    public float playerSpeed = 10;
    [SyncVar]
    public float gravity = 0.58f;
    [SyncVar]
    public float health = 100.0f;
    [SyncVar]
    public float water = 100.0f;

    [Command]
    public void CmdDoDamage(float amount)
    {
        health -= amount;
    }
}
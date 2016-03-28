using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class GameManager : NetworkBehaviour
{
    public static GameManager singleton;

    public List<PlayerFlower> flowers;
    public PlayerFlower[] tempFlowers = new PlayerFlower[100];

    public GameObject redFlower;
    public GameObject blueFlower;

    private void Start()
    {
        flowers = new List<PlayerFlower>();
        singleton = this;
    }

    private void OnGUI()
    {
        GUILayout.Box(flowers.Count.ToString());
    }

    [Command]
    public void CmdFlowerChange(int i, TeamType team)
    {
        Debug.Log(flowers[i].team +"|"+ team);
        if (flowers[i].team != team)
            RpcFlowerChange(i, team);
    }

    [Command]
    public void CmdCreateFlower(Vector3 pos, Quaternion rot, TeamType team)
    {
        RpcCreateFlower(pos, rot, team);
    }

    [ClientRpc]
    public void RpcCreateFlower(Vector3 pos, Quaternion rot, TeamType team)
    {
        GameObject obj;
        if (team == TeamType.Red)
            obj = (GameObject)Instantiate(redFlower, pos, rot);
        else
            obj = (GameObject)Instantiate(blueFlower, pos, rot);

        PlayerFlower f = obj.GetComponent<PlayerFlower>();
        f.team = team;
        //flowers.Add(f);
    }

    [ClientRpc]
    public void RpcFlowerChange(int i, TeamType team)
    {
        Debug.Log("changing");
        Destroy(flowers[i]);

        GameObject obj;
        if (team == TeamType.Red)
            obj = (GameObject)Instantiate(redFlower, flowers[i].transform.position, flowers[i].transform.rotation);
        else
            obj = (GameObject)Instantiate(blueFlower, flowers[i].transform.position, flowers[i].transform.rotation);
        flowers[i] = obj.GetComponent<PlayerFlower>();
        flowers[i].team = team;
    }
}
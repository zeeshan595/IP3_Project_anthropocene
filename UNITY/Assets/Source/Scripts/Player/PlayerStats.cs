using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public enum TeamType
{
    Red,
    Blue
}

public class PlayerStats : NetworkBehaviour
{
    [SyncVar]
    public Character character;
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
    [SyncVar]
    private int timer = 120;

    private RectTransform waterUI;
    private RectTransform healthUI;
    private Text timerUI;

    private void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetup(Settings.team);
            waterUI = (RectTransform)GameObject.FindWithTag("WaterUI").transform;
            healthUI = (RectTransform)GameObject.FindWithTag("HealthUI").transform;
            timerUI = GameObject.FindWithTag("TimerUI").GetComponent<Text>();
        }
        if (isServer)
        {
            StartCoroutine(tickTimer());
        }
    }

    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            float currentTeamWater, maxTeamWater;
            if (team == TeamType.Blue)
            {
                currentTeamWater = GameManager.blueWater;
                maxTeamWater = GameManager.maxBlueWater;
            }
            else
            {
                currentTeamWater = GameManager.redWater;
                maxTeamWater = GameManager.maxRedWater;
            }
            waterUI.anchoredPosition = new Vector2(0, (currentTeamWater / maxTeamWater) * 110);
            waterUI.sizeDelta = new Vector2(100, (currentTeamWater / maxTeamWater) * 220);

            healthUI.anchoredPosition = new Vector2(0, (health / 100) * 50);
            healthUI.sizeDelta = new Vector2(100, (health / 100) * 100);

            timerUI.text = timer.ToString();
            
            if (timer <= 0)
            {
                GameManager.singleton.EndGame(GetComponent<PlayerNetwork>().playerCamera.gameObject);
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerWeapon>().enabled = false;
            }
        }
    }

    private void OnGUI()
    {
        //Debug.Log(GameManager.blueWater + "/" + GameManager.maxBlueWater);
    }

    private IEnumerator tickTimer()
    {
        yield return new WaitForSeconds(1);
        timer--;

        if (timer > 0)
            StartCoroutine(tickTimer());
    }

    [Command]
    private void CmdSetup(TeamType team)
    {
        this.team = team;
    }

    [Command]
    public void CmdDoDamage(float amount)
    {
        health -= amount;
    }

    [Command]
    public void CmdUpdateWater(float water)
    {
        this.water = water;
    }
}
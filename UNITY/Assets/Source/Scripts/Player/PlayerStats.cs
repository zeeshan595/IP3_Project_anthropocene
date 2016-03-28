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
    private Weapon wep;

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
            if (wep != null)
            {
                waterUI.anchoredPosition = new Vector2(0, (water / wep.waterTank) * 110);
                waterUI.sizeDelta = new Vector2(100, (water / wep.waterTank) * 220);

                healthUI.anchoredPosition = new Vector2(0, (health / 100) * 50);
                healthUI.sizeDelta = new Vector2(100, (health / 100) * 100);

                timerUI.text = timer.ToString();
            }
            else
            {
                wep = GetComponent<PlayerWeapon>().currentWeapon;
            }
        }
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
}
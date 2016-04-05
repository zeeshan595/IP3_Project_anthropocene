using UnityEngine;
using UnityEngine.Networking;

public class WaterTank : NetworkBehaviour
{
    [SyncVar]
    public float water = 5600;
    [SyncVar]
    public TeamType team = TeamType.Red;
    public GameObject particleEffect;

    private float maxWater;

    [SerializeField]
    private GameObject waterMesh;

    private void OnTriggerEnter(Collider other)
    {
        if (isServer && other.gameObject.tag == "Player")
        {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
            Weapon weapon = other.gameObject.GetComponent<PlayerWeapon>().currentWeapon;
            if (player.team == team)
            {
                while (player.water < weapon.waterTank && water > 0)
                {
                    player.water++;
                    water--;
                }
            }
        }
    }

    private void Start()
    {
        if (team == Settings.team)
            particleEffect.SetActive(true);
        else
            particleEffect.SetActive(false);

        maxWater = water;
        GameManager.teamWater.Add(this);
    }

    private void Update()
    {
        float ratio = water / maxWater;
        waterMesh.transform.localPosition = new Vector3(0, (ratio / 2) - 0.2f, 0);
        waterMesh.transform.localScale = new Vector3(2.1f, ratio, 2.1f);
    }
}
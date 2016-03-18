using UnityEngine;
using UnityEngine.Networking;

public class WaterTank : NetworkBehaviour
{
    public float water = 5600;
    public TeamType team = TeamType.Red;

    private void OnTriggerEnter(Collider other)
    {
        if (isServer && other.gameObject.tag == "Player")
        {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
            Weapon weapon = other.gameObject.GetComponent<PlayerWeapon>().currentWeapon;
            if (player.team == team)
            {
                while (player.water < weapon.waterTank || water == 0)
                {
                    player.water++;
                    water--;
                }
            }
        }
    }
}
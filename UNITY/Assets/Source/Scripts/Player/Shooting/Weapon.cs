using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite crosshair;
    public WeaponType type = WeaponType.ScatterGun;
    public float range = 100.0f;
    public float damage = 25.0f;
    public float waterUsage = 10.0f;
    public float waterTank = 100.0f;
    public float acuracy = 0.0f;
    public float spray = 0.0f;
    public bool explode = false;
    public float rateOfFire = 0;
    public Transform barrel;
}
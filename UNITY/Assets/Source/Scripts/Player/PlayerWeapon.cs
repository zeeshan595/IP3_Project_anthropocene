using System;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField]
    private Weapon[] weapons;
    [SerializeField]
    private Transform weaponTransform;
    [SerializeField]
    private GameObject redFlower;
    [SerializeField]
    private GameObject blueFlower;

    private Weapon currentWeapon;
    private bool fireButtonReleased = false;
    private Transform playerCamera;
    private PlayerStats player;
    private int currentWeaponID = 0;

    private void Start()
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].type == Settings.weaponType)
                {
                    CmdCreateWeapon(i);
                    currentWeaponID = i;
                    break;
                }
            }
            playerCamera = Camera.main.gameObject.transform;
            player = GetComponent<PlayerStats>();
        }
        else
        {
            enabled = false;
        }
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (currentWeapon != null)
        {
            Debug.DrawRay(currentWeapon.barrel.position, playerCamera.forward);

            //Check If Button is pressed
            bool firePressed = InputManager.GetAxies(ControllerAxies.RightTrigger) > 0.5f;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                firePressed = true;
            }

            if (firePressed && (fireButtonReleased || currentWeapon.automatic))
            {
                //Create Raycast
                Ray ray = new Ray(currentWeapon.barrel.position, playerCamera.forward);
                RaycastHit[] hit = Physics.RaycastAll(ray, currentWeapon.range);
                if (hit.Length > 0)
                {
                    //If hitting something
                    //Sort array 0 = near, 10 = far
                    Array.Sort(hit, delegate (RaycastHit hit1, RaycastHit hit2)
                    {
                        return hit1.distance.CompareTo(hit2.distance);
                    });

                    if (hit[0].collider.tag == "Player")
                    {
                        hit[0].collider.gameObject.GetComponent<PlayerStats>().CmdDoDamage(currentWeapon.damage);
                    }
                    else if (hit[0].collider.tag != "Flower")
                    {
                        if (player.team == TeamType.Red)
                        {
                            CmdHitSomething(hit[0].point + (hit[0].normal * 0.1f), hit[0].normal, true, playerControllerId);
                            Instantiate(redFlower, hit[0].point + (hit[0].normal * 0.1f), Quaternion.LookRotation(hit[0].normal));
                        }
                        else
                        {
                            CmdHitSomething(hit[0].point + (hit[0].normal * 0.1f), hit[0].normal, false, playerControllerId);
                            Instantiate(blueFlower, hit[0].point + (hit[0].normal * 0.1f), Quaternion.LookRotation(hit[0].normal));
                        }
                    }
                }

                fireButtonReleased = false;
            }
            else if (!firePressed)
                fireButtonReleased = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentWeaponID++;
            if (currentWeaponID >= weapons.Length)
                currentWeaponID = 0;

            CmdResetWeapon(currentWeaponID);

        }
        if (Input.GetKeyDown(KeyCode.O))
        {

        }
    }

    [Command]
    private void CmdResetWeapon(int id)
    {
        RpcResetWeapon(id);
    }

    [ClientRpc]
    private void RpcResetWeapon(int id)
    {
        Destroy(currentWeapon.gameObject);
        GameObject w = (GameObject)Instantiate(weapons[id].gameObject, weaponTransform.position, weaponTransform.rotation);
        currentWeapon = w.GetComponent<Weapon>();
        Debug.Log(currentWeapon.type.ToString());
        w.transform.SetParent(weaponTransform);
    }

    [Command]
    private void CmdHitSomething(Vector3 point, Vector3 normal, bool isRed, short controlID)
    {
        RpcHitSomething(point, normal, isRed, controlID);
    }

    [Command]
    private void CmdCreateWeapon(int id)
    {
        RpcCreateWeapon(id);
    }

    [ClientRpc]
    private void RpcCreateWeapon(int id)
    {
        GameObject w = (GameObject)Instantiate(weapons[id].gameObject, weaponTransform.position, weaponTransform.rotation);
        currentWeapon = w.GetComponent<Weapon>();
        w.transform.SetParent(weaponTransform);
    }

    [ClientRpc]
    private void RpcHitSomething(Vector3 point, Vector3 normal, bool isRed, short controlID)
    {
        if (this.playerControllerId == controlID)
            return;

        if (isRed)
            Instantiate(redFlower, point, Quaternion.LookRotation(normal));
        else
            Instantiate(blueFlower, point, Quaternion.LookRotation(normal));
    }
}
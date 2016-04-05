using System.Collections;
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
    private GameObject bullet;

    public Weapon currentWeapon;
    private bool fireButtonReleased = false;
    private Transform playerCamera;
    private PlayerStats player;
    private int currentWeaponID = 0;
    private bool firePressed = false;

    private IEnumerator Start()
    {
        player = GetComponent<PlayerStats>();
        if (isLocalPlayer)
        {
            playerCamera = Camera.main.gameObject.transform;
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].type == Settings.weaponType)
                {
                    CmdCreateWeapon(i);
                    currentWeaponID = i;
                    break;
                }
            }
            StartCoroutine(Shoot());
        }
        else
        {
            enabled = false;
        }
        
    }

    private void Update()
    {
        //Check If Button is pressed
        firePressed = InputManager.GetAxies(ControllerAxies.RightTrigger) > 0.5f;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            firePressed = true;
        }
#if UNITY_EDITOR
        ChangeWeapon();
#endif
    }

    private IEnumerator Shoot()
    {
        if (currentWeapon != null)
        {
            if (firePressed && (fireButtonReleased || currentWeapon.rateOfFire > 0) && player.water > 0)
            {
                for (int i = 0; i < currentWeapon.spray; i++)
                {
                    Vector3 pos = currentWeapon.barrel.transform.position;
                    Vector3 acc = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0);
                    acc = currentWeapon.barrel.TransformDirection(acc) * currentWeapon.acuracy;
                    Quaternion rot = Quaternion.LookRotation(currentWeapon.barrel.transform.forward + acc);
                    CmdCreateBullet(pos, rot, currentWeaponID);
                    player.water -= currentWeapon.waterUsage;
                }
                fireButtonReleased = false;
            }
            else if (!firePressed)
                fireButtonReleased = true;

            if (currentWeapon.rateOfFire > 0)
                yield return new WaitForSeconds(1 / currentWeapon.rateOfFire);
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(Shoot());
    }

    #region UNITY_EDITOR
    //Designers
#if UNITY_EDITOR
    private void ChangeWeapon()
    {
        //For designers
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentWeaponID++;
            if (currentWeaponID >= weapons.Length)
                currentWeaponID = 0;

            CmdResetWeapon(currentWeaponID);

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentWeaponID--;
            if (currentWeaponID < 0)
                currentWeaponID = weapons.Length - 1;

            CmdResetWeapon(currentWeaponID);
        }
    }
#endif
    #endregion

    #region Server Commands

    [Command]
    private void CmdResetWeapon(int id)
    {
        player.water = weapons[id].waterTank;
        RpcResetWeapon(id);
    }

    [Command]
    private void CmdCreateWeapon(int id)
    {
        player.water = weapons[id].waterTank;
        RpcCreateWeapon(id);
    }

    [Command]
    private void CmdCreateBullet(Vector3 pos, Quaternion rot, int weaponID)
    {
        RpcCreateBullet(pos, rot, weaponID);
    }

    #endregion

    #region Client RPC

    [ClientRpc]
    private void RpcResetWeapon(int id)
    {
        Destroy(currentWeapon.gameObject);
        GameObject w = (GameObject)Instantiate(weapons[id].gameObject, weaponTransform.position, weaponTransform.rotation);
        currentWeapon = w.GetComponent<Weapon>();
        Debug.Log(currentWeapon.type.ToString());
        w.transform.SetParent(weaponTransform);
    }

    [ClientRpc]
    private void RpcCreateWeapon(int id)
    {
        /*
        switch ((WeaponType)id)
        {
            case WeaponType.WaterRake:
 
                break;
            case WeaponType.HoseGun:
                weaponTransform.SetParent(GetComponent<PlayerMeshUpdator>().mesh.GetComponent<MeshSetup>().hose);
                break;
            case WeaponType.ScatterGun:
                weaponTransform.SetParent(GetComponent<PlayerMeshUpdator>().mesh.GetComponent<MeshSetup>().scaterGun);
                break;
            case WeaponType.WaterBazooka:
                weaponTransform.SetParent(GetComponent<PlayerMeshUpdator>().mesh.GetComponent<MeshSetup>().bazooka);
                break;
        }
        */
        //weaponTransform.localPosition = Vector3.zero;
        GameObject w = (GameObject)Instantiate(weapons[id].gameObject, weaponTransform.position, weaponTransform.rotation);
        currentWeapon = w.GetComponent<Weapon>();
        w.transform.SetParent(weaponTransform);
    }

    [ClientRpc]
    private void RpcCreateBullet(Vector3 pos, Quaternion rot, int weaponID)
    {
        GameObject clone = (GameObject)Instantiate(bullet, pos, rot);
        Rigidbody body = clone.GetComponent<Rigidbody>();
        body.AddForce(clone.transform.forward * weapons[weaponID].range * 20);
        PlayerBullet bull = clone.GetComponent<PlayerBullet>();
        bull.team = player.team;
        bull.explode = weapons[weaponID].explode;
    }

    #endregion
}
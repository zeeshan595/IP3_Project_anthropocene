using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField]
    private GameObject redSeed;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject rayOrigin;
    [SerializeField]
    private GameObject waterEffect;

    private PlayerStats player;
    private RaycastHit hit;
    private bool isPlanted = false;
    private bool isWaterEffectOn = false;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            bool LeftTrigger = InputManager.GetAxies(ControllerAxies.LeftTrigger) > 0.5f;
            bool RightTrigger = InputManager.GetAxies(ControllerAxies.RightTrigger) > 0.5f;

            Ray ray = new Ray(rayOrigin.transform.position, playerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * player.gunRange, Color.red);
            if (RightTrigger || Input.GetKey(KeyCode.Mouse0))
            {
                if (!isPlanted)
                {
                    if (Physics.Raycast(ray, out hit, player.gunRange))
                    {
                        CmdServerInstantiate(hit.point);
                        isPlanted = true;
                    }
                }
            }
            else
                isPlanted = false;

            if (LeftTrigger || Input.GetKey(KeyCode.Mouse1))
            {
                if (!isWaterEffectOn)
                {
                    waterEffect.GetComponent<ParticleSystem>().Play();
                    isWaterEffectOn = true;
                }
            }
            else
            {
                waterEffect.GetComponent<ParticleSystem>().Stop();
                isWaterEffectOn = false;
            }
        }
    }

    [Command]
    private void CmdServerInstantiate(Vector3 pos)
    {
        RpcClientsInstantiate(pos);
    }

    [ClientRpc]
    private void RpcClientsInstantiate(Vector3 pos)
    {
        Instantiate(redSeed, pos, Quaternion.identity);
    }
}
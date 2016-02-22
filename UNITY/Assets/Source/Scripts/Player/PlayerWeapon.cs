using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] redPlants;
    [SerializeField]
    private GameObject[] bluePlants;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject rayOrigin;
    [SerializeField]
    private GameObject waterEffect;

    private PlayerStats player;
    private bool isPlanted = false;
    private bool isWaterEffectOn = false;
    private GameObject crossHair;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
        crossHair = GameObject.Find("Canvas").transform.FindChild("Cross Hair").gameObject;
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
                    RaycastHit[] hit = Physics.RaycastAll(ray, player.gunRange);
                    System.Array.Sort(hit, delegate (RaycastHit hit1, RaycastHit hit2)
                    {
                        return hit1.distance.CompareTo(hit2.distance);
                    });

                    if (hit.Length > 0)
                    {
                        if (hit[0].collider.tag != "Player")
                        {
                            for (int i = 0; i < hit.Length; i++)
                            {
                                if (hit[i].collider.tag == "Flower")
                                {
                                    Destroy(hit[i].collider.gameObject);
                                }
                                else
                                {
                                    CmdServerInstantiate(hit[i].point, hit[i].normal);
                                    isPlanted = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                isPlanted = false;
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, player.gunRange))
                {
                    ((RectTransform)crossHair.transform).anchoredPosition = Camera.main.WorldToScreenPoint(hit.point);
                }
            }

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
    private void CmdServerInstantiate(Vector3 pos, Vector3 normal)
    {
        RpcClientsInstantiate(pos, normal);
    }

    [ClientRpc]
    private void RpcClientsInstantiate(Vector3 pos, Vector3 normal)
    {
        GameObject flower;
        if (GetComponent<PlayerStats>().team == TeamType.Blue)
            flower = (GameObject)Instantiate(bluePlants[Random.Range(0, bluePlants.Length)], pos + (normal * 0.05f), Quaternion.LookRotation(normal));
        else
            flower = (GameObject)Instantiate(redPlants[Random.Range(0, redPlants.Length)], pos + (normal * 0.05f), Quaternion.LookRotation(normal));


    }
}
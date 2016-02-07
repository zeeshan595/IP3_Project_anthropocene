using UnityEngine;

public class PlayerWeapon : MonoBehaviour
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
        bool LeftTrigger = InputManager.GetAxies(ControllerAxies.LeftTrigger) > 0.5f;
        bool RightTrigger = InputManager.GetAxies(ControllerAxies.RightTrigger) > 0.5f;

        Ray ray = new Ray(rayOrigin.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * player.gunRange, Color.red);
        if (RightTrigger)
        {
            if (!isPlanted)
            {
                if (Physics.Raycast(ray, out hit, player.gunRange))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        Instantiate(redSeed, hit.point, Quaternion.identity);
                        isPlanted = true;
                    }
                }
            }
        }
        else
            isPlanted = false;

        if (LeftTrigger)
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
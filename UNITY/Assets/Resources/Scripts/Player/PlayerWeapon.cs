using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject redSeed;
    public GameObject blueSeed;
    public GameObject waterEffect;

    private PlayerStats playerStats;
    private bool isPlayingEffect = false;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        bool LeftTrigger = InputManager.GetAxies(ControllerAxies.LeftTrigger) > 0.5f;
        bool RightTrigger = InputManager.GetAxies(ControllerAxies.RightTrigger) > 0.5f;


        Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(ray.origin, ray.direction * playerStats.gunRange, Color.red);

        if (RightTrigger)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, playerStats.gunRange))
            {
                if (playerStats.team == TeamType.Red)
                {
                    Instantiate(redSeed, hit.point, Quaternion.identity);
                }
            }
        }
        if (LeftTrigger)
        {
            if (!isPlayingEffect)
            {
                waterEffect.GetComponent<ParticleSystem>().Play();
                isPlayingEffect = true;
            }
        }
        else
        {
            isPlayingEffect = false;
            waterEffect.GetComponent<ParticleSystem>().Stop();
        }
    }
}
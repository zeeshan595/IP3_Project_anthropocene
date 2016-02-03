using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject playerCamera;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        bool LeftTrigger = InputManager.GetAxies(ControllerAxies.LeftTrigger) > 0.5f;

        Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit rayCastHit;
        if (Physics.Raycast(ray, out rayCastHit, playerStats.gunRange))
        {
            Debug.Log(rayCastHit.collider.name);
        }
    }
}
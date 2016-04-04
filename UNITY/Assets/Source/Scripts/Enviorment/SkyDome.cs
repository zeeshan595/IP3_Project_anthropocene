using UnityEngine;

public class SkyDome : MonoBehaviour
{
    private Material mat;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    private void LateUpdate()
    {
        int currentFlowers = GameManager.flowers.Count;
        currentFlowers = Mathf.Clamp(currentFlowers, 0, 2000);
        float ratio = 1.0f - ((float)currentFlowers / 2000.0f);
        mat.color = new Color(1.0f, 1.0f, 1.0f, ratio);
    }
}
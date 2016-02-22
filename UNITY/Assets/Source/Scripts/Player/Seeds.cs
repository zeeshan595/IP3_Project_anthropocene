using UnityEngine;
using System.Collections;

public class Seeds : MonoBehaviour
{
    public GameObject redFlower;

    private void OnParticleCollision(GameObject other)
    {
        Instantiate(redFlower, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class PlayerMeshUpdator : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] characters;
    [SerializeField]
    private GameObject meshParent;

    [System.NonSerialized]
    public GameObject mesh;

    private void Start()
    {
        int i = (int)GetComponent<PlayerStats>().character;
        mesh = (GameObject)Instantiate(characters[i], meshParent.transform.position, meshParent.transform.rotation);
        mesh.transform.SetParent(meshParent.transform);
    }
}
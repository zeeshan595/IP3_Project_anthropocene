using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class PlayerMeshUpdator : NetworkBehaviour
{
    [SerializeField]
    private Chars[] characters;
    [SerializeField]
    private Transform meshParent;

    [System.NonSerialized]
    public GameObject mesh;

    private void Start()
    {
        int i = (int)GetComponent<PlayerStats>().character;
        mesh = (GameObject)Instantiate(characters[i].mesh, meshParent.transform.position, meshParent.transform.rotation);
        mesh.transform.SetParent(meshParent.transform);
        meshParent.transform.localPosition = characters[i].offset;
        GetComponent<CharacterController>().height = characters[i].height;
    }
}

[System.Serializable]
public class Chars
{
    public GameObject mesh;
    public Texture2D blueTexture;
    public Texture2D redTexture;
    public int height;
    public Vector3 offset;
}
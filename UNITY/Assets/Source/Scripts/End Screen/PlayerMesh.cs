using UnityEngine;

public class PlayerMesh : MonoBehaviour
{
    [SerializeField]
    private Chars[] characters;

    [System.NonSerialized]
    public GameObject mesh;

    private void Start()
    {
        int i = (int)Settings.character;
        mesh = (GameObject)Instantiate(characters[i].mesh, transform.position, transform.rotation);
        mesh.transform.SetParent(transform);
    }
}
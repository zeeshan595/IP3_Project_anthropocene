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
        foreach (Renderer r in mesh.GetComponentsInChildren<Renderer>())
        {
            if (Settings.team == TeamType.Blue)
                r.material.mainTexture = characters[i].blueTexture;
            else
                r.material.mainTexture = characters[i].redTexture;
        }
    }
}
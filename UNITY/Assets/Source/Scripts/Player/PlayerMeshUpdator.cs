using UnityEngine;

public class PlayerMeshUpdator : MonoBehaviour
{
    [SerializeField]
    public Transform[] guns;
    [SerializeField]
    private Chars[] characters;
    [SerializeField]
    private Transform meshParent;

    [System.NonSerialized]
    public GameObject mesh;

    private PlayerStats player;
    private WaterMesh waterMesh;

    private void Start()
    {
        int c = (int)GetComponent<PlayerStats>().character;
        mesh = (GameObject)Instantiate(characters[c].mesh, meshParent.transform.position, meshParent.transform.rotation);
        mesh.transform.SetParent(meshParent.transform);
        meshParent.transform.localPosition = characters[c].offset;
        GetComponent<CharacterController>().height = characters[c].height;
        Renderer[] renders = mesh.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            if (renders[i].gameObject.tag != "Jetpack")
            {
                if (Settings.team == TeamType.Blue)
                {
                    if (characters[c].blueTexture)
                        renders[i].material.mainTexture = characters[c].blueTexture;
                }
                else
                {
                    if (characters[c].redTexture)
                        renders[i].material.mainTexture = characters[c].redTexture;
                }
            }
        }

        guns = new Transform[4];
        WeaponMesh[] wMeshes = mesh.GetComponentsInChildren<WeaponMesh>();
        if (wMeshes.Length == 4)
        {
            for (int k = 0; k < wMeshes.Length; k++)
            {
                if (wMeshes[k].type == WeaponType.ScatterGun)
                    guns[0] = wMeshes[k].transform;
                else if (wMeshes[k].type == WeaponType.HoseGun)
                    guns[1] = wMeshes[k].transform;
                else if (wMeshes[k].type == WeaponType.WaterRake)
                    guns[2] = wMeshes[k].transform;
                else
                    guns[3] = wMeshes[k].transform;
            }
        }

        player = GetComponent<PlayerStats>();
        waterMesh = gameObject.GetComponentsInChildren<WaterMesh>()[0];
    }

    private void Update()
    {
        waterMesh.water = Mathf.RoundToInt(player.water);
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
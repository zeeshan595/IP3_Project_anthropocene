using UnityEngine;
using System.Collections;

public class MeshUpdator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] guns;

    private Transform[] gundummys;
    private GameObject currentGun;

    private void Start()
    {
        gundummys = new Transform[4];
        WeaponMesh[] wMeshes = GetComponentsInChildren<WeaponMesh>();
        if (wMeshes.Length == 4)
        {
            for (int k = 0; k < wMeshes.Length; k++)
            {
                if (wMeshes[k].type == WeaponType.ScatterGun)
                    gundummys[0] = wMeshes[k].transform;
                else if (wMeshes[k].type == WeaponType.HoseGun)
                    gundummys[1] = wMeshes[k].transform;
                else if (wMeshes[k].type == WeaponType.WaterRake)
                    gundummys[2] = wMeshes[k].transform;
                else
                    gundummys[3] = wMeshes[k].transform;
            }
        }

        WeaponChanged();
    }

    public void WeaponChanged()
    {
        if (currentGun != null)
            Destroy(currentGun);

		currentGun = (GameObject)Instantiate(guns[(int)Settings.weaponType], gundummys[(int)Settings.weaponType].position, gundummys[(int)Settings.weaponType].rotation);
        currentGun.transform.SetParent(gundummys[(int)Settings.weaponType].transform);
		GetComponent<Animator> ().SetFloat ("Weapon", (int)Settings.weaponType);
    }
}
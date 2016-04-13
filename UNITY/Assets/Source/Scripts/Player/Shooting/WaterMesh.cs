using UnityEngine;
using System.Collections;

public class WaterMesh : MonoBehaviour
{
    public int water = 100;

    [SerializeField]
    private bool x;
    [SerializeField]
    private bool y;
    [SerializeField]
    private bool z;

    private void Update()
    {
        Vector3 size = Vector3.one;
        if (x)
            size.x = (float)water / 100.0f;
        if (y)
            size.y = (float)water / 100.0f;
        if (z)
            size.z = (float)water / 100.0f;
        transform.localScale = size;
    }
}
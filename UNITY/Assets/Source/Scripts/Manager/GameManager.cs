using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> flowers;

    private void Start()
    {
        flowers = new List<GameObject>();
    }
}
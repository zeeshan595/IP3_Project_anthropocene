using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public static List<GameObject> flowers = new List<GameObject>();

    private void Start()
    {
        singleton = this;
        flowers = new List<GameObject>();
    }

    private void OnGUI()
    {
        int red = 0;
        int blue = 0;
        for (int i = 0; i < flowers.Count; i++)
        {
            if (flowers[i].GetComponent<PlayerFlower>().team == TeamType.Red)
                red++;
            else
                blue++;
        }

        GUILayout.Box(red + "|" + blue);
    }
}
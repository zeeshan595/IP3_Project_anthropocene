using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public static List<GameObject> flowers = new List<GameObject>();
    public static int blueWater = 7600;
    public static int redWater = 7600;

    private Transform cam;

    private void Start()
    {
        singleton = this;
        flowers = new List<GameObject>();
        blueWater = 7600;
        redWater = 7600;
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
        GUILayout.Box("Red: " + (((float)red / ((float)red + (float)blue + 1.0f)) * 100.0f));
        GUILayout.Box("Blue: " + (((float)blue / ((float)red + (float)blue + 1.0f)) * 100.0f));
    }

    public void EndGame(GameObject camera)
    {
        camera.GetComponent<PlayerCamera>().enabled = false;
        cam = camera.transform;
    }

    private void Update()
    {
        if (cam != null)
        {
            cam.position = Vector3.Lerp(cam.position, new Vector3(-30, 60, 8), Time.deltaTime);
            cam.rotation = Quaternion.Lerp(cam.rotation, Quaternion.Euler(new Vector3(90, 0, 0)), Time.deltaTime);
        }
    }
}
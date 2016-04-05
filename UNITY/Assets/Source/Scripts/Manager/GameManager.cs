using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public static List<GameObject> flowers = new List<GameObject>();
    public static List<WaterTank> teamWater = new List<WaterTank>();
    public static float blueWater = 0;
    public static float redWater = 0;
    public static float maxBlueWater = 7600;
    public static float maxRedWater = 7600;

    private Transform cam;

    private void Awake()
    {
        singleton = this;
        flowers = new List<GameObject>();
        teamWater = new List<WaterTank>();
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
        float tempBlue = 0, tempRed = 0;
        for (int i = 0; i < teamWater.Count; i++)
        {
            if (teamWater[i].team == TeamType.Blue)
            {
                tempBlue += teamWater[i].water;
            }
            else
            {
                tempRed += teamWater[i].water;
            }
        }

        if (tempBlue > blueWater)
        {
            maxBlueWater = tempBlue;
        }
        if (tempRed > redWater)
        {
            maxRedWater = tempRed;
        }
        blueWater = tempBlue;
        redWater = tempRed;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public static List<GameObject> flowers = new List<GameObject>();
    public static List<WaterTank> teamWater = new List<WaterTank>();
    public static int redFlowers = 0, blueFlowers = 0;
    public static float blueWater = 0;
    public static float redWater = 0;
    public static float maxBlueWater = 7600;
    public static float maxRedWater = 7600;

    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private GameObject inGameUI;

    [SerializeField]
    private RectTransform bluePlant;
    [SerializeField]
    private RectTransform redPlant;

    private Transform cam;

    private void Awake()
    {
        singleton = this;
        flowers = new List<GameObject>();
        teamWater = new List<WaterTank>();
    }

    public void EndGame(GameObject camera)
    {
        camera.GetComponent<PlayerCamera>().enabled = false;
        cam = camera.transform;
        inGameUI.SetActive(false);
        endScreen.SetActive(true);
    }

    private void Update()
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
        redFlowers = red;
        blueFlowers = blue;

        if (cam != null)
        {
            cam.position = Vector3.Lerp(cam.position, new Vector3(-30, 60, 8), Time.deltaTime);
            cam.rotation = Quaternion.Lerp(cam.rotation, Quaternion.Euler(new Vector3(90, 0, 0)), Time.deltaTime);
            float redRatio = (float)redFlowers / ((float)redFlowers + (float)blueFlowers);
            float blueRatio = (float)blueFlowers / ((float)redFlowers + (float)blueFlowers);
            redRatio *= 300;
            blueRatio *= 300;
            redRatio += ((float)redWater / (float)(redWater + blueWater)) * 300;
            blueRatio += ((float)blueWater / (float)(redWater + blueWater)) * 300;
            redPlant.anchoredPosition = Vector2.Lerp(redPlant.anchoredPosition, new Vector2(0, redRatio - 600), Time.deltaTime);
            bluePlant.anchoredPosition = Vector2.Lerp(bluePlant.anchoredPosition, new Vector2(0, blueRatio - 600), Time.deltaTime);
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
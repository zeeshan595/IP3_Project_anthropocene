using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public static List<GameObject> flowers = new List<GameObject>();
    public static List<WaterTank> teamWater = new List<WaterTank>();
    public static int redFlowers = 0, blueFlowers = 0;
    public static float blueWater = 0;
    public static float redWater = 0;
    public static float bluePercent = 0;
    public static float redPercent = 0;
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

    [SerializeField]
    private Text blueText;
    [SerializeField]
    private Text redText;

    [SerializeField]
    private Image winImage;

    [SerializeField]
    public Sprite[] blueVictoryDefeat;
    [SerializeField]
    private Sprite[] redVictoryDefeat;

    private Transform cam;
    private bool waterStuff = false;
    private bool startAnimation = false;

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
        Invoke("StartAnim", 2);
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
            redText.text = Mathf.RoundToInt(redPercent) + "%";
            blueText.text = Mathf.RoundToInt(bluePercent) + "%";

            if (startAnimation)
            {
                float redRatio = (float)redFlowers / ((float)redFlowers + (float)blueFlowers);
                float blueRatio = (float)blueFlowers / ((float)redFlowers + (float)blueFlowers);
                if (waterStuff)
                {
                    redRatio += ((float)redWater / (float)(redWater + blueWater));
                    blueRatio += ((float)blueWater / (float)(redWater + blueWater));
                    redPercent = redRatio * 50;
                    bluePercent = blueRatio * 50;
                    redRatio *= 300;
                    blueRatio *= 300;
                    redPlant.anchoredPosition = Vector2.Lerp(redPlant.anchoredPosition, new Vector2(0, redRatio - 600), Time.deltaTime);
                    bluePlant.anchoredPosition = Vector2.Lerp(bluePlant.anchoredPosition, new Vector2(0, blueRatio - 600), Time.deltaTime);
                    winImage.gameObject.SetActive(true);
                    if (redPercent > bluePercent)
                    {
                        if (Settings.team == TeamType.Red)
                        {
                            winImage.sprite = redVictoryDefeat[0];
                        }
                        else
                        {
                            winImage.sprite = blueVictoryDefeat[1];
                        }
                    }
                    else
                    {
                        if (Settings.team == TeamType.Blue)
                        {
                            winImage.sprite = blueVictoryDefeat[0];
                        }
                        else
                        {
                            winImage.sprite = redVictoryDefeat[1];
                        }
                    }
                }
                else
                {
                    redPercent = Mathf.Lerp(redPercent, redRatio * 50, Time.deltaTime * 0.5f);
                    bluePercent = Mathf.Lerp(bluePercent, blueRatio * 50, Time.deltaTime * 0.5f);
                    if (redPlant.anchoredPosition.y >= (redRatio * 300) - 600 || bluePlant.anchoredPosition.y >= (blueRatio * 300) - 600)
                    {
                        Invoke("Splash", 1.5f);
                    }
                    else
                    {
                        redPlant.anchoredPosition += Vector2.up * Time.deltaTime * 5 * redPercent;
                        bluePlant.anchoredPosition += Vector2.up * Time.deltaTime * 5 * bluePercent;
                    }
                }
            }
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

    private void StartAnim()
    {
        startAnimation = true;
    }

    private void Splash()
    {
        waterStuff = true;
    }
}
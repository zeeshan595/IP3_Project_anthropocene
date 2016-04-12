using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class EndGame : MonoBehaviour
{

    private List<Player> redTeam;
    private List<Player> blueTeam;

    //Sorry...
    public GameObject[] winningTeamObjects;
    public GameObject[] loserTeamObjects;
    public GameObject[] winningTeamScore;
    public GameObject[] loserTeamScore;
    public GameObject[] winningKills;
    public GameObject[] loserKills;
    public GameObject[] winningDeaths;
    public GameObject[] loserDeaths;
    public Sprite[] characterIcons; // 0 = Potaetree 1 = Rak 2 = Fishy 3 = JackieChan
    public Sprite[] redIcons;
    public GameObject[] winningCharacterIconsObjects;
    public GameObject[] loserCharacterIconsObjects;

    public GameObject mask;
    private float redTeamTotalWaterUsage;
    private float blueTeamTotalWaterUsage;



    // Use this for initialization
    void Start()
    {
        redTeam = new List<Player>();
        blueTeam = new List<Player>();

        RectTransform rect = mask.GetComponent<RectTransform>();
        foreach (Player p in GameManager.Players)
        {
            if (p.team == TeamType.Red)
            {
                redTeam.Add(p);
                redTeamTotalWaterUsage += p.waterUsage;
                if (p.username == Settings.username)
                {
                    float playerPercent = p.waterUsage / redTeamTotalWaterUsage * 100;
                    float heightPercent = (rect.offsetMax.y / rect.offsetMax.y) * 100;
                    //rect.offsetMax.y = playerPercent / heightPercent;
                }
            }
            else
            {
                blueTeam.Add(p);
                blueTeamTotalWaterUsage += p.waterUsage;
                if (p.username == Settings.username)
                {
                    float playerPercent = (p.waterUsage / blueTeamTotalWaterUsage) * 100;
                    float heightPercent = (rect.offsetMax.y / rect.offsetMax.y) * 100;
                    //rect.offsetMax.y = playerPercent / heightPercent;
                }
            }
        }

        //Sort teams by score
        QuickSort(redTeam);
        QuickSort(blueTeam);

        //RED TEAM WINS
        if (GameManager.redPercent > GameManager.bluePercent)
        {
            //THIS WILL ONLY WORK IF THEY HAVE THE SAME NUMBER OF PLAYERS ON EACH TEAM
            for (int i = 0; i < winningTeamObjects.Length; i++)
            {
                //Winning Team usernames
                Text winningUsername = winningTeamObjects[i].GetComponent<Text>();
                winningUsername.text = redTeam[i].username;

                //Loser Team usernames
                Text loserUsername = loserTeamObjects[i].GetComponent<Text>();
                loserUsername.text = blueTeam[i].username;

                //Winning Team Score
                Text winningScore = winningTeamScore[i].GetComponent<Text>();
                winningScore.text = redTeam[i].score.ToString();

                //Loser Team Score
                Text loserScore = loserTeamScore[i].GetComponent<Text>();
                loserScore.text = blueTeam[i].score.ToString();

                //Winning Team kills
                Text winningTeamKills = winningKills[i].GetComponent<Text>();
                winningTeamKills.text = redTeam[i].kills.ToString();

                //Loser Team kills
                Text loserTeamKills = loserKills[i].GetComponent<Text>();
                loserTeamKills.text = blueTeam[i].kills.ToString();

                //Winning Team Deaths
                Text winningTeamDeaths = winningDeaths[i].GetComponent<Text>();
                winningTeamDeaths.text = redTeam[i].deaths.ToString();

                //Loser Team Deaths
                Text loserTeamDeaths = loserDeaths[i].GetComponent<Text>();
                loserTeamDeaths.text = blueTeam[i].deaths.ToString();

                //Assign Icon
                if (redTeam[i].character == Character.Potatree)
                {
                    Image im = winningCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = redIcons[0];
                }
                else if (redTeam[i].character == Character.Rak)
                {
                    Image im = winningCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = redIcons[1];
                }
                else if (redTeam[i].character == Character.Fishy)
                {
                    Image im = winningCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = redIcons[2];
                }

                //Assign Icon
                if (blueTeam[i].character == Character.Potatree)
                {
                    Image im = loserCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = characterIcons[0];
                }
                else if (blueTeam[i].character == Character.Rak)
                {
                    Image im = loserCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = characterIcons[1];
                }
                else if (blueTeam[i].character == Character.Fishy)
                {
                    Image im = loserCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = characterIcons[2];
                }

                //Winning Team Kills
                Text winningKillsText = winningKills[i].GetComponent<Text>();
                winningKillsText.text = redTeam[i].kills.ToString();

                //Loser Team Kills
                Text loserKillsText = loserKills[i].GetComponent<Text>();
                loserKillsText.text = blueTeam[i].kills.ToString();

                //Winning Team Deaths
                Text winningDeathsText = winningDeaths[i].GetComponent<Text>();
                winningDeathsText.text = redTeam[i].kills.ToString();

                //Loser Team Deaths
                Text loserDeathsText = loserDeaths[i].GetComponent<Text>();
                loserDeathsText.text = blueTeam[i].kills.ToString();
            }
        }
        //BLUE TEAM WINS
        else
        {
            //THIS WILL ONLY WORK IF THEY HAVE THE SAME NUMBER OF PLAYERS ON EACH TEAM
            for (int i = 0; i < winningTeamObjects.Length; i++)
            {
                //Winning Team Username
                Text winningUsername = winningTeamObjects[i].GetComponent<Text>();
                winningUsername.text = blueTeam[i].username;

                //Loser Team Username
                Text loserUsername = loserTeamObjects[i].GetComponent<Text>();
                loserUsername.text = redTeam[i].username;

                //Winning Team Score
                Text winningScore = winningTeamScore[i].GetComponent<Text>();
                winningScore.text = blueTeam[i].score.ToString();

                //Loser Team Score
                Text loserScore = loserTeamScore[i].GetComponent<Text>();
                loserScore.text = redTeam[i].score.ToString();

                //Winning Team kills
                Text winningTeamKills = winningKills[i].GetComponent<Text>();
                winningTeamKills.text = blueTeam[i].kills.ToString();

                //Loser Team kills
                Text loserTeamKills = loserKills[i].GetComponent<Text>();
                loserTeamKills.text = redTeam[i].kills.ToString();

                //Winning Team Deaths
                Text winningTeamDeaths = winningDeaths[i].GetComponent<Text>();
                winningTeamDeaths.text = blueTeam[i].deaths.ToString();

                //Loser Team Deaths
                Text loserTeamDeaths = loserDeaths[i].GetComponent<Text>();
                loserTeamDeaths.text = redTeam[i].deaths.ToString();

                //Assign Icon
                if (blueTeam[i].character == Character.Potatree)
                {
                    Image im = winningCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = characterIcons[0];
                }
                else if (blueTeam[i].character == Character.Rak)
                {
                    Image im = winningCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = characterIcons[1];
                }
                else if (blueTeam[i].character == Character.Fishy)
                {
                    Image im = winningCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = characterIcons[2];
                }

                //Assign Icon
                if (redTeam[i].character == Character.Potatree)
                {
                    Image im = loserCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = redIcons[0];
                }
                else if (redTeam[i].character == Character.Rak)
                {
                    Image im = loserCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = redIcons[1];
                }
                else if (redTeam[i].character == Character.Fishy)
                {
                    Image im = loserCharacterIconsObjects[i].GetComponent<Image>();
                    im.sprite = redIcons[2];
                }


                //Winning Team Kills
                Text winningKillsText = winningKills[i].GetComponent<Text>();
                winningKillsText.text = blueTeam[i].kills.ToString();

                //Loser Team Kills
                Text loserKillsText = loserKills[i].GetComponent<Text>();
                loserKillsText.text = redTeam[i].kills.ToString();

                //Winning Team Deaths
                Text winningDeathsText = winningDeaths[i].GetComponent<Text>();
                winningDeathsText.text = blueTeam[i].kills.ToString();

                //Loser Team Deaths
                Text loserDeathsText = loserDeaths[i].GetComponent<Text>();
                loserDeathsText.text = redTeam[i].kills.ToString();
            }
        }
    }

    //Sort list by score
    private List<Player> QuickSort(List<Player> unsortedList)
    {
        List<Player> sortedList = new List<Player>();
        List<Player> greaterList = new List<Player>();
        List<Player> lesserList = new List<Player>();
        System.Random r = new System.Random();
        int pivotPos = r.Next(unsortedList.Count);
        Player pivot = unsortedList[pivotPos];

        unsortedList.RemoveAt(pivotPos);

        for (int i = 0; i < unsortedList.Count; i++)
        {
            if (unsortedList[i].score <= pivot.score)
            {
                lesserList.Add(unsortedList[i]);
            }
            else
            {
                greaterList.Add(unsortedList[i]);
            }
        }
        sortedList.AddRange(QuickSort(lesserList));
        sortedList.Add(pivot);
        sortedList.AddRange(greaterList);
        return sortedList;
    }
}
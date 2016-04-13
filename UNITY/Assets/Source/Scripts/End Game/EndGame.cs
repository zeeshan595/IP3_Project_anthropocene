using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private Sprite[] characterImagesRed;
    [SerializeField]
    private Sprite[] characterImagesBlue;

    [SerializeField]
    private Text points;

    [SerializeField]
    private Text[] winnerUsernames;
    [SerializeField]
    private Text[] winnerScores;
    [SerializeField]
    private Image[] winnerCharacters;
    [SerializeField]
    private Text[] winnerWaterUsage;

    [SerializeField]
    private Text[] loserUsernames;
    [SerializeField]
    private Text[] loserScores;
    [SerializeField]
    private Image[] loserCharacters;
    [SerializeField]
    private Text[] loserWaterUsage;

    private List<Player> winList = new List<Player>();
    private List<Player> loseList = new List<Player>();
    private int me;

    private void Start()
    {
        //Re-order them with best scores
        for (int i = 0; i < GameManager.Players.Length; i++)
        {
            //Store my results for later
            if (Settings.username == GameManager.Players[i].username)
            {
                me = i;
            }

            for (int k = i; k < GameManager.Players.Length; k++)
            {
                if (GameManager.Players[i].score < GameManager.Players[k].score)
                {
                    Player temp = GameManager.Players[i];
                    GameManager.Players[i] = GameManager.Players[k];
                    GameManager.Players[k] = temp;
                }
            }
        }

        //Seperate winning and losing teams
        TeamType winningTeam = TeamType.Red;
        if (GameManager.redPercent < GameManager.bluePercent)
            winningTeam = TeamType.Blue;
        for (int i = 0; i < GameManager.Players.Length; i++)
        {
            if (GameManager.Players[i].team == winningTeam)
            {
                winList.Add(GameManager.Players[i]);
            }
            else
            {
                loseList.Add(GameManager.Players[i]);
            }
        }

        //Update UI
        for (int i = 0; i < winList.Count; i++)
        {
            winnerUsernames[i].text = winList[i].username;
            winnerScores[i].text = winList[i].score.ToString();
            if (winList[i].team == TeamType.Red)
                winnerCharacters[i].sprite = characterImagesRed[(int)winList[i].character];
            else
                winnerCharacters[i].sprite = characterImagesBlue[(int)winList[i].character];
            winnerWaterUsage[i].text = winList[i].waterUsage.ToString();
        }
        for (int i = 0; i < loseList.Count; i++)
        {
            loserUsernames[i].text = loseList[i].username;
            loserScores[i].text = loseList[i].score.ToString();
            if (loseList[i].team == TeamType.Red)
                loserCharacters[i].sprite = characterImagesRed[(int)loseList[i].character];
            else
                loserCharacters[i].sprite = characterImagesBlue[(int)loseList[i].character];
            loserWaterUsage[i].text = loseList[i].waterUsage.ToString();
        }

        points.text = "Points: " + GameManager.Players[me].score;
    }
}
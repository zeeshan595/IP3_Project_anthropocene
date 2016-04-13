using UnityEngine;

public class TotalTeamWater : MonoBehaviour
{
    private void Start()
    {
        if (Settings.team == TeamType.Blue)
        {
            float ratio = (GameManager.blueWater / GameManager.maxBlueWater) * 600.0f;
            ((RectTransform)transform.GetChild(0).transform).anchoredPosition = new Vector2(0, ratio);
            ((RectTransform)transform).anchoredPosition = new Vector2(0, -ratio);
        }
        else
        {
            float ratio = (GameManager.redWater / GameManager.maxRedWater) * 600.0f;
            ((RectTransform)transform.GetChild(0).transform).anchoredPosition = new Vector2(0, ratio);
            ((RectTransform)transform).anchoredPosition = new Vector2(0, -ratio);
        }
    }
}
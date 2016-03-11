using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuPlayButton : MonoBehaviour {

    public void PlayButton()
    {
        SceneManager.LoadScene("Lobby");
    }
}

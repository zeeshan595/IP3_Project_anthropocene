using UnityEngine;
using System.Collections;

public struct ResolutionOptions
{
    public int[] resolution; //0 = width, 1= height
    public bool isFullScreen;
}

public class OptionsMenu : MonoBehaviour {

    //Options Panels
    public GameObject graphicsPanel;
    public GameObject soundPanel;
    public GameObject controllsPanel;

    public int[,] res = new int[,] {{1280,720}, {1360,768}, {1366,768}, {1600,900}, {1920,1080} };
    public GameObject[] resOptions;
    
    //Current options
    public ResolutionOptions currentOptions;


	// Use this for initialization
	void Start () {
        currentOptions.resolution = new int[2];
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //TODO change this to be done via the inspector in the Unity Editor
    public void graphicPanelButton()
    {
        graphicsPanel.SetActive(true);
        soundPanel.SetActive(false);
        controllsPanel.SetActive(false);
    }

    public void soundPanelButton()
    {
        graphicsPanel.SetActive(false);
        soundPanel.SetActive(true);
        controllsPanel.SetActive(false);
    }

    public void controllsPanelButton()
    {
        graphicsPanel.SetActive(false);
        soundPanel.SetActive(false);
        controllsPanel.SetActive(true);
    }

    public void SetResolution(int index)
    {
        currentOptions.resolution[0] = res[index, 0];
        currentOptions.resolution[1] = res[index, 1];
    }

    public void SetFullScreen(bool value)
    {
        currentOptions.isFullScreen = value;
    }

    public void ApplyOptions()
    {
        Screen.SetResolution(currentOptions.resolution[0], currentOptions.resolution[1], currentOptions.isFullScreen);
    }
}

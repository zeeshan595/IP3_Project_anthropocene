using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour {

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    public GameObject masterVolumeSliderObject;
    public GameObject musicVolumeSliderObject;
    public GameObject sfxVolumeSliderObject;

    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;

    void Start()
    {
        masterVolumeSlider = masterVolumeSliderObject.GetComponent<Slider>();
        musicVolumeSlider = musicVolumeSliderObject.GetComponent<Slider>();
        sfxVolumeSlider = sfxVolumeSliderObject.GetComponent<Slider>();
    }

    public void ApplyButton()
    {
        masterVolume = masterVolumeSlider.value;
        musicVolume = musicVolumeSlider.value;
        sfxVolume = sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("Master Volume", masterVolume);
        PlayerPrefs.SetFloat("Music Volume", musicVolume);
        PlayerPrefs.SetFloat("SFX Volume", sfxVolume);
    }
}

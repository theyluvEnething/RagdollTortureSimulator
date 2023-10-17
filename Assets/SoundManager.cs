using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; 

    void Start()
    {
        if (!PlayerPrefs.HasKey("global_volume"))
        {
            PlayerPrefs.SetFloat("global_volume", 0.5f);
            Load();
        } else {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value * 2f;
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("global_volume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("global_volume", volumeSlider.value);
    }
}

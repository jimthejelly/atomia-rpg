using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;


[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(AudioSource))]
public class VolumeSliderScript : MonoBehaviour, IPointerUpHandler
{

    private AudioSource audioData;

    private Slider volumeSlider;
    private float volumeLevel;

    public TMPro.TextMeshProUGUI volumeLabel;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        audioData = GetComponent<AudioSource>();
        AudioListener.volume = 0.01F;
        volumeLevel = volumeSlider.value;
        volumeLabel.text = Math.Round(volumeLevel * 100).ToString();
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateVolume(float value)
    {
        volumeLevel = value;
        AudioListener.volume = volumeLevel;
        volumeLabel.text = Math.Round(volumeLevel * 100).ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        audioData.Play();
    }
}

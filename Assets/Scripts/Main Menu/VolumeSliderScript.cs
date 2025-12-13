using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;


[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(AudioSource))]
public class VolumeSliderScript : MonoBehaviour, IPointerUpHandler, IMoveHandler
{

    int counter = 0;

    private AudioSource audioData;

    private Slider volumeSlider;
    private float volumeLevel;

    public float stepSize = 0.01f;
    public float defaultStepSize = 0.1f;

    public TMPro.TextMeshProUGUI volumeLabel;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        audioData = GetComponent<AudioSource>();
        AudioListener.volume = 0.01F;
        volumeLevel = volumeSlider.value;
        volumeLabel.text = Math.Round(volumeLevel * 100).ToString();
        volumeLabel.rectTransform.anchoredPosition = new Vector2(volumeLabel.rectTransform.anchoredPosition.x, -20f);
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 0)
        {
            counter--;
        }

        if (EventSystem.current.currentSelectedGameObject == volumeSlider.gameObject)
        {
            CheckForVolumeKeys();
        }
    }

    private void CheckForVolumeKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)){
            volumeSlider.value = 0.0f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
            volumeSlider.value = 0.1f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)){
            volumeSlider.value = 0.2f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)){
            volumeSlider.value = 0.3f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)){
            volumeSlider.value = 0.4f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)){
            volumeSlider.value = 0.5f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)){
            volumeSlider.value = 0.6f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)){
            volumeSlider.value = 0.7f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)){
            volumeSlider.value = 0.8f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)){
            volumeSlider.value = 0.9f;
            OnPointerUp(null);
        }else if (Input.GetKeyDown(KeyCode.F)){
            volumeSlider.value = 1.0f;
            OnPointerUp(null);
        }
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

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            volumeSlider.value -= stepSize;
            volumeSlider.value += defaultStepSize;
            if (counter == 0)
            {
                counter = 40;
                OnPointerUp(null);
            }
        }
        else if (eventData.moveDir == MoveDirection.Right)
        {
            volumeSlider.value += stepSize;
            volumeSlider.value -= defaultStepSize;
            if (counter == 0)
            {
                counter = 40;
                OnPointerUp(null);
            }
        }
    }



}

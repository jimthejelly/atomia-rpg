using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardNavigationScript : MonoBehaviour
{
    public Slider volumeSlider;
    public Button saveButton;
    private Outline sliderOutline;
    private Outline buttonOutline;
    private GameObject sliderGO;
    private GameObject buttonGO;

    void Awake()
    {
        SetupNavigation();
        SetupOutlines();
    }
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(sliderGO);
    }
    
    void OnDisable()
    {
        if (sliderOutline != null) sliderOutline.enabled = false;
        if (buttonOutline != null) buttonOutline.enabled = false;
    }

    void Update()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected == sliderGO){
            sliderOutline.enabled = true;
            buttonOutline.enabled = false;
        }else if (currentSelected == buttonGO){
            sliderOutline.enabled = false;
            buttonOutline.enabled = true;
        }else{
            sliderOutline.enabled = false;
            buttonOutline.enabled = false;
        }
    }

    void SetupNavigation()
    {

        Navigation navSlider = volumeSlider.navigation;
        navSlider.mode = Navigation.Mode.Explicit;
        navSlider.selectOnDown = saveButton;
        volumeSlider.navigation = navSlider;

        Navigation navButton = saveButton.navigation;
        navButton.mode = Navigation.Mode.Explicit;
        navButton.selectOnUp = volumeSlider;
        saveButton.navigation = navButton;
    }

    void SetupOutlines()
    {
        sliderGO = volumeSlider.gameObject;
        buttonGO = saveButton.gameObject;

        sliderOutline = sliderGO.GetComponent<Outline>();
        if (sliderOutline == null)
        {
            sliderOutline = sliderGO.AddComponent<Outline>();
        }

        buttonOutline = buttonGO.GetComponent<Outline>();
        if (buttonOutline == null)
        {
            buttonOutline = buttonGO.AddComponent<Outline>();
        }

        Color redColor = Color.red;
        Vector2 outlineDistance = new Vector2(2f, -2f);

        sliderOutline.effectColor = redColor;
        sliderOutline.effectDistance = outlineDistance;
        
        buttonOutline.effectColor = redColor;
        buttonOutline.effectDistance = outlineDistance;

        sliderOutline.enabled = false;
        buttonOutline.enabled = false;
    }
}
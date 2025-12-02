using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class KeyboardNavigationScript : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropdown;
    public Button saveButton;
    public Button loadButton;

    private Outline sliderOutline;
    private Outline dropdownOutline;
    private Outline saveOutline;
    private Outline loadOutline;

    private GameObject sliderGO;
    private GameObject dropdownGO;
    private GameObject saveGO;
    private GameObject loadGO;

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
        if (dropdownOutline != null) dropdownOutline.enabled = false;
        if (saveOutline != null) saveOutline.enabled = false;
        if (loadOutline != null) loadOutline.enabled = false;
    }

    void Update()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected == sliderGO)
        {
            EnableOutline(sliderOutline);
        }
        else if (currentSelected == dropdownGO)
        {
            EnableOutline(dropdownOutline);
        }
        else if (currentSelected == saveGO)
        {
            EnableOutline(saveOutline);
        }
        else if (currentSelected == loadGO)
        {
            EnableOutline(loadOutline);
        }
        else
        {
            DisableAllOutlines();
        }
    }

    void SetupNavigation()
    {
        // Volume slider navigation
        Navigation navSlider = volumeSlider.navigation;
        navSlider.mode = Navigation.Mode.Explicit;
        navSlider.selectOnDown = qualityDropdown;
        volumeSlider.navigation = navSlider;

        // Quality dropdown navigation
        Navigation navDropdown = qualityDropdown.navigation;
        navDropdown.mode = Navigation.Mode.Explicit;
        navDropdown.selectOnUp = volumeSlider;
        navDropdown.selectOnDown = saveButton;
        qualityDropdown.navigation = navDropdown;

        // Save button navigation
        Navigation navSave = saveButton.navigation;
        navSave.mode = Navigation.Mode.Explicit;
        navSave.selectOnUp = qualityDropdown;
        navSave.selectOnDown = loadButton;
        saveButton.navigation = navSave;

        // Load button navigation
        Navigation navLoad = loadButton.navigation;
        navLoad.mode = Navigation.Mode.Explicit;
        navLoad.selectOnUp = saveButton;
        loadButton.navigation = navLoad;
    }

    void SetupOutlines()
    {
        sliderGO = volumeSlider.gameObject;
        dropdownGO = qualityDropdown.gameObject;
        saveGO = saveButton.gameObject;
        loadGO = loadButton.gameObject;

        sliderOutline = GetOrAddOutline(sliderGO);
        dropdownOutline = GetOrAddOutline(dropdownGO);
        saveOutline = GetOrAddOutline(saveGO);
        loadOutline = GetOrAddOutline(loadGO);

        Color redColor = Color.red;
        Vector2 outlineDistance = new Vector2(2f, -2f);

        sliderOutline.effectColor = redColor;
        dropdownOutline.effectColor = redColor;
        saveOutline.effectColor = redColor;
        loadOutline.effectColor = redColor;

        sliderOutline.effectDistance = outlineDistance;
        dropdownOutline.effectDistance = outlineDistance;
        saveOutline.effectDistance = outlineDistance;
        loadOutline.effectDistance = outlineDistance;

        DisableAllOutlines();
    }

    Outline GetOrAddOutline(GameObject go)
    {
        Outline outline = go.GetComponent<Outline>();
        if (outline == null)
        {
            outline = go.AddComponent<Outline>();
        }
        return outline;
    }

    void EnableOutline(Outline active)
    {
        sliderOutline.enabled = (active == sliderOutline);
        dropdownOutline.enabled = (active == dropdownOutline);
        saveOutline.enabled = (active == saveOutline);
        loadOutline.enabled = (active == loadOutline);
    }

    void DisableAllOutlines()
    {
        sliderOutline.enabled = false;
        dropdownOutline.enabled = false;
        saveOutline.enabled = false;
        loadOutline.enabled = false;
    }
}

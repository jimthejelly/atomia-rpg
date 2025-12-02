using System.Collections.Generic;
using System.Linq; // We'll use this to get the quality names easily
using UnityEngine;
using TMPro; // Don't forget this!

// This line ensures the script has access to the dropdown it's attached to
[RequireComponent(typeof(TMP_Dropdown))]
public class QualityDropdownSetup : MonoBehaviour
{
    private TMP_Dropdown qualityDropdown;

    void Start()
    {
        // 1. Get the dropdown component that is on this same GameObject
        qualityDropdown = GetComponent<TMP_Dropdown>();

        // 2. Clear any options you might have added in the Inspector
        qualityDropdown.ClearOptions();

        // 3. Get the list of quality names from Project Settings
        string[] qualityNames = QualitySettings.names;
        List<string> options = new List<string>(qualityNames);

        // 4. Add the quality names to the dropdown
        qualityDropdown.AddOptions(options);

        // 5. Set the dropdown's current value to match the game's quality
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        // 6. Add a "listener" to call our function when the value changes
        // This is the programmatic way of setting up the "On Value Changed" event
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    // This is the function the listener will call
    public void SetQuality(int qualityIndex)
    {
        // This is the magic line that changes the project's quality
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // (Good practice) Remove the listener when the object is destroyed
    void OnDestroy()
    {
        if (qualityDropdown != null)
        {
            qualityDropdown.onValueChanged.RemoveListener(SetQuality);
        }
    }
}
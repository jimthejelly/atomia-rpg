using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementButtonRedux : MonoBehaviour
{
    // letters to set for elements
    public string elementSymbol;

    private TitrationManagerRedux titrationManager;
    private Text displayText;
    private Weighting.Element elementData;

    void Start()
    {
        Debug.Log($"ElementButtonRedux starting on {gameObject.name}");

        titrationManager = FindObjectOfType<TitrationManagerRedux>();
        Debug.Log($"Found TitrationManagerRedux: {titrationManager != null}");
        displayText = GetComponentInChildren<Text>();
        
        if (string.IsNullOrEmpty(elementSymbol))
        {
            Debug.LogError($"ElementButtonRedux on {gameObject.name}: elementSymbol is not set!");
            return;
        }
        
        elementData = Weighting.instance.GetElement(elementSymbol);
        
        if (elementData == null)
        {
            Debug.LogError($"ElementButtonRedux: Could not find element '{elementSymbol}'");
            return;
        }
        
        UpdateDisplay();
    }

    private void OnMouseDown()
    {
        Debug.Log($"Button clicked! Element: {elementSymbol}");

        if (titrationManager != null)
        {
            titrationManager.AddElementLeft(elementSymbol);
        }
        else
        {
            Debug.LogError("TitrationManagerRedux not found in scene");
        }
    }

    private void UpdateDisplay()
    {
        if (displayText != null && elementData != null)
        {
            displayText.text = elementData.symbol;
        }
    }
}
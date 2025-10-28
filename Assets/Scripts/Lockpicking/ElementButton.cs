using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this to each element icon button in your titration minigame.
/// When clicked, it adds the element to the beaker via TitrationManager.
/// </summary>
public class ElementButton : MonoBehaviour
{
    [Header("Element Properties")]
    [Tooltip("The chemical symbol (e.g., 'H', 'O', 'Na', 'Cl')")]
    public string elementSymbol;
    
    [Tooltip("The charge of this element when added (e.g., +1, -1, +2)")]
    public int elementCharge = 0;
    
    [Tooltip("Optional: Element name for display")]
    public string elementName;

    [Header("References")]
    [Tooltip("The TitrationManager controlling this minigame")]
    public TitrationManager titrationManager;

    private Button button;

    void Start()
    {
        // Get the Button component
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("ElementButton requires a Button component!");
            return;
        }

        // Add click listener
        button.onClick.AddListener(OnElementClicked);

        // Find TitrationManager if not assigned
        if (titrationManager == null)
        {
            titrationManager = FindObjectOfType<TitrationManager>();
            if (titrationManager == null)
            {
                Debug.LogError("ElementButton couldn't find TitrationManager in scene!");
            }
        }
    }

    /// <summary>
    /// Called when the element button is clicked
    /// </summary>
    void OnElementClicked()
    {
        if (titrationManager != null)
        {
            titrationManager.AddElement(elementSymbol, elementCharge, elementName);
            Debug.Log($"Added element: {elementSymbol} (charge: {elementCharge})");
        }
        else
        {
            Debug.LogError("TitrationManager reference is missing!");
        }
    }

    void OnDestroy()
    {
        // Clean up listener when destroyed
        if (button != null)
        {
            button.onClick.RemoveListener(OnElementClicked);
        }
    }
}

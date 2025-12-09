using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This serves to be attached to the element icon buttons in the minigame
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

    void Start()
    {
        Debug.Log($"ElementButton Start() called on {gameObject.name}");

        // Find TitrationManager if not assigned
        if (titrationManager == null)
        {
            titrationManager = FindObjectOfType<TitrationManager>();
            if (titrationManager == null)
            {
                Debug.LogError($"ElementButton on {gameObject.name} couldn't find TitrationManager in scene!");
            }
            else
            {
                Debug.Log($"TitrationManager found for {gameObject.name}");
            }
        }
        
        Debug.Log($"ElementButton ready: {gameObject.name} - Symbol: {elementSymbol}, Charge: {elementCharge}");
    }
    
    // For world-space sprites (SpriteRenderer) - detects mouse clicks
    void OnMouseDown()
    {
        Debug.Log($"Mouse clicked on {gameObject.name}!");
        OnElementClicked();
    }

    void OnElementClicked()
    {
        Debug.Log($"OnElementClicked fired for {gameObject.name}!");
        
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClickableWord : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textComponent;
    private Color originalColor;

    [SerializeField] private string elementName;
    [SerializeField] private GameObject spritePrefab;

    // Start is called before the first frame update
    void Start()
    {
        textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            originalColor = textComponent.color;
        }
        else
        {
            Debug.LogWarning($"TextMeshProUGUI component not found on {gameObject.name}");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{elementName} clicked!");
        if (spritePrefab != null)
        {
            Instantiate(spritePrefab, transform.position, Quaternion.identity);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change color on hover (optional)
        if (textComponent != null)
        {
            textComponent.color = Color.yellow; // Or any highlight color
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restore original color
        if (textComponent != null)
        {
            textComponent.color = originalColor;
        }
    }


}

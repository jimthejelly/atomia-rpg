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
    private Button button;
    private Weighting.Element elementData;

    // Start is called before the first frame update
    void Start()
    {
        titrationManager = FindObjectOfType<TitrationManagerRedux>();
        displayText = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        elementData = Weighting.instance.GetElement(elementSymbol);

        button.onClick.AddListener(OnButtonClicked);
        UpdateDisplay();
    }


    /* This function will add the element to the left beeker when clicked */
    private void OnButtonClicked()
    {
        titrationManager.AddElementLeft(elementSymbol);
    }

    /* will display the element's data when it is clicked */
    private void UpdateDisplay()
    {
        displayText.text = elementData.symbol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
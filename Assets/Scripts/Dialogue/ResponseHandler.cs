using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private GameObject popupBox;
    [SerializeField] private RectTransform responseContainer;
    [SerializeField] private GameObject OptionTemplate;
    
    [SerializeField] private TMP_Text responseOptionText;

    private List<GameObject> optionTemplates = new List<GameObject>();
    private List<TMP_Text> optionTexts = new List<TMP_Text>();
    private int currentSelection = 0;
    private Response[] currentResponses;

    private bool isActive = false;

    public delegate void ResponseSelectedDelegate(Response selected);
    private ResponseSelectedDelegate onResponseSelected;

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangeSelection(1);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangeSelection(-1);
        else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            SelectResponse();
    }

    public void ShowResponses(Response[] responses, ResponseSelectedDelegate callback)
    {
        currentResponses = responses;
        onResponseSelected = callback;

        ClearOptions();
        currentSelection = 0;

        foreach (Response response in responses)
        {
            GameObject optionTemp = Instantiate(OptionTemplate, responseContainer);
            optionTemplates.Add(optionTemp);

            TMP_Text optionText = optionTemp.GetComponentInChildren<TMP_Text>();
            optionText.text = response.ResponseText;
            optionTexts.Add(optionText);

            optionTemp.SetActive(true);
        }

        popupBox.SetActive(true);
        isActive = true;

        HighlightSelection();
    }

    private void ChangeSelection(int direction)
    {
        UnhighlightSelection();

        currentSelection += direction;
        if (currentSelection < 0) currentSelection = optionTexts.Count - 1;
        if (currentSelection >= optionTexts.Count) currentSelection = 0;

        HighlightSelection();
    }

    private void HighlightSelection()
    {
        if (optionTexts.Count > 0)
            optionTexts[currentSelection].color = Color.yellow;
    }

    private void UnhighlightSelection()
    {
        if (optionTexts.Count > 0)
            optionTexts[currentSelection].color = Color.white;
    }

    private void SelectResponse()
    {
        isActive = false;
        popupBox.SetActive(false);
        ClearOptions();

        onResponseSelected?.Invoke(currentResponses[currentSelection]);
    }

    private void ClearOptions()
    {
        foreach (GameObject option in optionTemplates)
        {
            Destroy(option.gameObject);
        }
        optionTexts.Clear();
        optionTemplates.Clear();
    }
}

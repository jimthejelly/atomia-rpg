using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuOptions : MonoBehaviour
{
    public TMP_Text[] menuOptions; // Assign in Inspector
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private int selectedIndex = 0;
    private bool inputLocked = false;

    void Start()
    {
        UpdateMenuSelection();
    }

    void Update()
    {
        if (!inputLocked)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedIndex = (selectedIndex + 1) % menuOptions.Length;
                UpdateMenuSelection();
                LockInput();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedIndex = (selectedIndex - 1 + menuOptions.Length) % menuOptions.Length;
                UpdateMenuSelection();
                LockInput();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                SelectOption();
                LockInput();
            }
        }
        
        if (inputLocked && (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Z)))
        {
            inputLocked = false;
        }
    }

    void UpdateMenuSelection()
    {
        for (int i = 0; i < menuOptions.Length; i++)
        {
            menuOptions[i].color = (i == selectedIndex) ? selectedColor : normalColor;
        }
    }

    void SelectOption()
    {
        switch (selectedIndex)
        {
            case 0:
                Debug.Log("Start Game");
                SceneManager.LoadScene("1_InitialConvo");
                break;
            case 1:
                Debug.Log("Options");
                break;
            case 2:
                Debug.Log("Quit");
                Application.Quit();
                break;
        }
    }

    void LockInput()
    {
        inputLocked = true;
    }
}

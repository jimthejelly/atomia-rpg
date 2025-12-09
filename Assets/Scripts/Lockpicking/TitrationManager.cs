using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Manages the titration minigame state. Tracks elements added to beaker,
/// calculates charge balance, and determines win/loss conditions.
/// </summary>
/* NEXT STEPS:
 - add timer to count damage
 - finish timer when win or loss */

public class TitrationManager : MonoBehaviour
{
    [System.Serializable]
    public class Element
    {
        public string symbol;
        public int charge;
        public string name;

        public Element(string symbol, int charge, string name = "")
        {
            this.symbol = symbol;
            this.charge = charge;
            this.name = name;
        }
    }

    [Header("Game Settings")]
    [Tooltip("Target charge to balance (usually 0 for neutral)")]
    public int targetCharge = 0;
    
    [Tooltip("How long to maintain balance to win (seconds)")]
    public float timeToWin = 3f;
    
    [Tooltip("Allowed deviation from target charge")]
    public int chargeDeviation = 0;

    [Header("UI References")]
    public TextMeshProUGUI currentChargeText;
    public TextMeshProUGUI elementsInBeakerText;
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Visual References")]
    public BalanceBeam balanceBeam;
    public Formula currentFormula;

    [Header("Events")]
    public UnityEvent onWin;
    public UnityEvent onLose;
    public UnityEvent<int> onChargeChanged; // Passes current charge

    // Current state
    private List<Element> elementsInBeaker = new List<Element>();
    private int currentCharge = 0;
    private float balanceTimer = 0f;
    private bool isBalanced = false;
    private bool gameEnded = false;
    private bool hasAddedElements = false;
    private float gameTimer = 0f;
    private bool TimeRun = false;

    void Start()
    {
        // Find BalanceBeam if not assigned
        if (balanceBeam == null)
        {
            balanceBeam = FindObjectOfType<BalanceBeam>();
        }

        UpdateUI();
        
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        // Check if currently balanced (only if elements have been added)
        isBalanced = hasAddedElements && Mathf.Abs(currentCharge - targetCharge) <= chargeDeviation;

        if (isBalanced)
        {
            balanceTimer += Time.deltaTime;
            
            if (timerText != null)
            {
                timerText.text = $"Balanced: {balanceTimer:F1}s / {timeToWin:F1}s";
            }

            // Win condition
            if (balanceTimer >= timeToWin)
            {
                bool formulaMatches = true;
                if (currentFormula != null && currentFormula.currentFormula != null)
                {
                    formulaMatches = currentFormula.currentFormula.CheckMatch(elementsInBeaker);
                }

                if (formulaMatches)
                {
                    WinGame();
                }
                else{
                    LoseGame();
                }
            }
        }
        else
        {
            balanceTimer = 0f;
            
            if (timerText != null)
            {
                timerText.text = "Not balanced";
            }
        }

        // Update balance beam visual
        if (balanceBeam != null)
        {
            balanceBeam.UpdateBalance(currentCharge, targetCharge);
        }
    }

    /// <summary>
    /// Add an element to the beaker
    /// </summary>
    public void AddElement(string symbol, int charge, string name = "")
    {
        if (gameEnded) return;

        hasAddedElements = true;
        Element newElement = new Element(symbol, charge, name);
        elementsInBeaker.Add(newElement);
        currentCharge += charge;

        UpdateUI();
        onChargeChanged?.Invoke(currentCharge);

        Debug.Log($"Added {symbol} (charge: {charge}). Total charge: {currentCharge}");
    }

    /// <summary>
    /// Remove the last added element (undo functionality)
    /// </summary>
    public void RemoveLastElement()
    {
        if (gameEnded || elementsInBeaker.Count == 0) return;

        Element lastElement = elementsInBeaker[elementsInBeaker.Count - 1];
        currentCharge -= lastElement.charge;
        elementsInBeaker.RemoveAt(elementsInBeaker.Count - 1);

        UpdateUI();
        onChargeChanged?.Invoke(currentCharge);

        Debug.Log($"Removed {lastElement.symbol}. Total charge: {currentCharge}");
    }

    /// <summary>
    /// Clear all elements from beaker (reset)
    /// </summary>
    public void ClearBeaker()
    {
        if (gameEnded) return;

        elementsInBeaker.Clear();
        currentCharge = 0;
        balanceTimer = 0f;

        UpdateUI();
        onChargeChanged?.Invoke(currentCharge);

        Debug.Log("Beaker cleared");
    }

    /// <summary>
    /// Update all UI elements
    /// </summary>
    void UpdateUI()
    {
        // Update charge display
        if (currentChargeText != null)
        {
            string chargeSign = currentCharge > 0 ? "+" : "";
            currentChargeText.text = $"Charge: {chargeSign}{currentCharge}";
        }

        // Update elements list
        if (elementsInBeakerText != null)
        {
            string elementsList = "Beaker: ";
            if (elementsInBeaker.Count == 0)
            {
                elementsList += "Empty";
            }
            else
            {
                for (int i = 0; i < elementsInBeaker.Count; i++)
                {
                    elementsList += elementsInBeaker[i].symbol;
                    if (i < elementsInBeaker.Count - 1)
                    {
                        elementsList += ", ";
                    }
                }
            }
            elementsInBeakerText.text = elementsList;
        }
    }

    void WinGame()
    {
        gameEnded = true;
        Debug.Log("Titration balanced! You win!");
        
        if (winPanel != null) winPanel.SetActive(true);
        onWin?.Invoke();
    }

    public void LoseGame()
    {
        gameEnded = true;
        Debug.Log("Titration failed!");
        
        if (losePanel != null) losePanel.SetActive(true);
        onLose?.Invoke();
    }

    public void ResetGame()
    {
        gameEnded = false;
        hasAddedElements = false;
        ClearBeaker();
        balanceTimer = 0f;
        
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        
        UpdateUI();
    }

    // Public getters for other scripts
    public int GetCurrentCharge() => currentCharge;
    public bool IsBalanced() => isBalanced;
    public float GetBalanceTimer() => balanceTimer;
    public List<Element> GetElementsInBeaker() => new List<Element>(elementsInBeaker);
}

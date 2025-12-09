using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Represents a chemical formula that the player needs to balance.
/// Works with TitrationManager to check if the player's element combination matches.
/// </summary>
[System.Serializable]
public class FormulaData
{
    public string formulaName;           // e.g., "Water"
    public string formulaText;           // e.g., "H₂O"
    public string description;           // e.g., "Two hydrogen, one oxygen"
    
    [Header("Required Elements")]
    public List<ElementRequirement> requiredElements = new List<ElementRequirement>();
    
    [System.Serializable]
    public class ElementRequirement
    {
        public string symbol;     // e.g., "H", "O"
        public int quantity;      // e.g., 2 for H₂
        public int charge;        // e.g., +1 for H⁺
    }
    
    /// <summary>
    /// Check if the player's beaker matches this formula
    /// </summary>
    public bool CheckMatch(List<TitrationManager.Element> playerElements)
    {
        Dictionary<string, int> playerCounts = new Dictionary<string, int>();
        foreach (var elem in playerElements)
        {
            if (playerCounts.ContainsKey(elem.symbol))
                playerCounts[elem.symbol]++;
            else
                playerCounts[elem.symbol] = 1;
        }
        
        // Check if player's count matches the required elements
        if (playerCounts.Count != requiredElements.Count)
            return false;
            
        foreach (var requirement in requiredElements)
        {
            if (!playerCounts.ContainsKey(requirement.symbol))
                return false;
            if (playerCounts[requirement.symbol] != requirement.quantity)
                return false;
        }
        
        return true;
    }
    
    // <summary>
    // Get the total charge this formula should have
    // </summary>
    public int GetTargetCharge()
    {
        int total = 0;
        foreach (var req in requiredElements)
        {
            total += req.charge * req.quantity;
        }
        return total;
    }
}

public class Formula : MonoBehaviour
{
    [Header("Current Formula")]
    public FormulaData currentFormula;
    
    [Header("Formula Library")]
    public List<FormulaData> availableFormulas = new List<FormulaData>();
    
    [Header("UI References")]
    public TextMeshProUGUI formulaDisplayText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI hintText;
    
    [Header("References")]
    public TitrationManager titrationManager;
    
    private int currentFormulaIndex = 0;

    void Start()
    {
        if (titrationManager == null)
        {
            titrationManager = FindObjectOfType<TitrationManager>();
        }
        
       if (availableFormulas.Count == 0)
        {
            CreateDefaultFormulas();
        }
        
        // Load first formula
        if (availableFormulas.Count > 0)
        {
            LoadFormula(0);
        }
    }

    void Update()
    {
        // Check if player has completed the formula
        if (titrationManager != null && currentFormula != null)
        {
            var playerElements = titrationManager.GetElementsInBeaker();
            
            if (currentFormula.CheckMatch(playerElements))
            {
                if (hintText != null)
                {
                    hintText.text = "✓ Formula matched!";
                    hintText.color = Color.green;
                }
            }
            else if (playerElements.Count > 0)
            {
                if (hintText != null)
                {
                    hintText.text = "Keep trying...";
                    hintText.color = Color.yellow;
                }
            }
        }
    }
    
    public void LoadFormula(int index)
    {
        if (index < 0 || index >= availableFormulas.Count)
            return;
            
        currentFormulaIndex = index;
        currentFormula = availableFormulas[index];
        
        // Update UI
        if (formulaDisplayText != null)
        {
            formulaDisplayText.text = currentFormula.formulaText;
        }
        
        if (descriptionText != null)
        {
            descriptionText.text = currentFormula.description;
        }
        
        if (hintText != null)
        {
            hintText.text = "Add elements to match the formula";
            hintText.color = Color.white;
        }
        
        if (titrationManager != null)
        {
            titrationManager.targetCharge = currentFormula.GetTargetCharge();
        }
        
        Debug.Log($"Loaded formula: {currentFormula.formulaName} ({currentFormula.formulaText})");
    }
    
    public void NextFormula()
    {
        currentFormulaIndex = (currentFormulaIndex + 1) % availableFormulas.Count;
        LoadFormula(currentFormulaIndex);
        
        // Reset the beaker
        if (titrationManager != null)
        {
            titrationManager.ResetGame();
        }
    }
    
    public void PreviousFormula()
    {
        currentFormulaIndex--;
        if (currentFormulaIndex < 0)
            currentFormulaIndex = availableFormulas.Count - 1;
        LoadFormula(currentFormulaIndex);
        
        // Reset the beaker
        if (titrationManager != null)
        {
            titrationManager.ResetGame();
        }
    }
    
 
    void CreateDefaultFormulas()
    {
        // Water (H₂O)
        FormulaData water = new FormulaData();
        water.formulaName = "Water";
        water.formulaText = "H₂O";
        water.description = "2 Hydrogen + 1 Oxygen";
        water.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "H", quantity = 2, charge = 1 });
        water.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "O", quantity = 1, charge = -2 });
        availableFormulas.Add(water);
        
        // Ammonia (NH₃)
        FormulaData ammonia = new FormulaData();
        ammonia.formulaName = "Ammonia";
        ammonia.formulaText = "NH₃";
        ammonia.description = "1 Nitrogen + 3 Hydrogen";
        ammonia.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "N", quantity = 1, charge = -3 });
        ammonia.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "H", quantity = 3, charge = 1 });
        availableFormulas.Add(ammonia);
        
        // Methane (CH₄)
        FormulaData methane = new FormulaData();
        methane.formulaName = "Methane";
        methane.formulaText = "CH₄";
        methane.description = "1 Carbon + 4 Hydrogen";
        methane.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "C", quantity = 1, charge = -4 });
        methane.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "H", quantity = 4, charge = 1 });
        availableFormulas.Add(methane);
        
        // Boron Nitride (BN)
        FormulaData boronNitride = new FormulaData();
        boronNitride.formulaName = "Boron Nitride";
        boronNitride.formulaText = "BN";
        boronNitride.description = "1 Boron + 1 Nitrogen";
        boronNitride.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "B", quantity = 1, charge = 3 });
        boronNitride.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "N", quantity = 1, charge = -3 });
        availableFormulas.Add(boronNitride);
        
        // Boron Trioxide (B₂O₃)
        FormulaData boronTrioxide = new FormulaData();
        boronTrioxide.formulaName = "Boron Trioxide";
        boronTrioxide.formulaText = "B₂O₃";
        boronTrioxide.description = "2 Boron + 3 Oxygen";
        boronTrioxide.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "B", quantity = 2, charge = 3 });
        boronTrioxide.requiredElements.Add(new FormulaData.ElementRequirement { symbol = "O", quantity = 3, charge = -2 });
        availableFormulas.Add(boronTrioxide);
        
        Debug.Log($"Created {availableFormulas.Count} default formulas");
    }
}

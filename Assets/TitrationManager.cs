using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the titration minigame - tracks added elements and updates the balance beam
/// </summary>
public class TitrationManager : MonoBehaviour
{
    [Header("References")]
    public BalanceBeam balanceBeam;
    
    [Header("Balance Goal")]
    [Tooltip("Target charge to achieve (usually 0 for neutral)")]
    public int targetCharge = 0;
    
    // Track current charges on each side
    private int leftSideCharge = 0;
    private int rightSideCharge = 0;

    private void Start()
    {
        if (balanceBeam == null)
        {
            balanceBeam = GetComponent<BalanceBeam>();
        }
    }

    /// <summary>
    /// Add an element to the left side of the beam
    /// </summary>
    public void AddElementLeft(string elementSymbol)
    {
        Weighting.Element element = Weighting.instance.GetElement(elementSymbol);
        if (element != null)
        {
            leftSideCharge += element.charge;
            UpdateBeam();
            Debug.Log($"Added {element.name} to left side. Left charge: {leftSideCharge}");
        }
    }

    /// <summary>
    /// Add an element to the right side of the beam
    /// </summary>
    public void AddElementRight(string elementSymbol)
    {
        Weighting.Element element = Weighting.instance.GetElement(elementSymbol);
        if (element != null)
        {
            rightSideCharge += element.charge;
            UpdateBeam();
            Debug.Log($"Added {element.name} to right side. Right charge: {rightSideCharge}");
        }
    }

    /// <summary>
    /// Calculate total charge difference and update the beam
    /// </summary>
    private void UpdateBeam()
    {
        // Calculate which side is heavier
        int totalChargeDifference = leftSideCharge - rightSideCharge;
        
        // Negative difference = right is heavier, positive = left is heavier
        balanceBeam.UpdateBalance(totalChargeDifference, targetCharge);
    }

    /// <summary>
    /// Check if the beam is balanced
    /// </summary>
    public bool IsBalanced()
    {
        return balanceBeam.IsBalanced();
    }

    /// <summary>
    /// Reset the minigame
    /// </summary>
    public void Reset()
    {
        leftSideCharge = 0;
        rightSideCharge = 0;
        UpdateBeam();
        Debug.Log("Titration reset");
    }

    /// <summary>
    /// Get current charge info (useful for debugging)
    /// </summary>
    public void PrintChargeInfo()
    {
        Debug.Log($"Left: {leftSideCharge} | Right: {rightSideCharge} | Difference: {leftSideCharge - rightSideCharge}");
    }
}

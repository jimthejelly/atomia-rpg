using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Visual representation of the titration balance.
/// Rotates or moves based on charge balance (0-180 scale).
/// Position 90 = perfectly balanced.
/// </summary>
public class BalanceBeam : MonoBehaviour
{
    [Header("Balance Settings")]
    [Tooltip("Furthest left position (most negative charge)")]
    public int minPosition = 0;
    
    [Tooltip("Furthest right position (most positive charge)")]
    public int maxPosition = 180;
    
    [Tooltip("Center/balanced position")]
    public int centerPosition = 90;
    
    [Tooltip("How many charge units to shift full scale")]
    public int chargeRange = 10;

    [Header("Visual Settings")]
    [Tooltip("Should the beam rotate (Z-axis) or translate (X-axis)?")]
    public bool useRotation = true;
    
    [Tooltip("Smoothing speed for movement")]
    public float smoothSpeed = 5f;

    [Header("Color Feedback")]
    public SpriteRenderer beamRenderer;
    public Color balancedColor = Color.green;
    public Color unbalancedColor = Color.red;
    public bool useColorFeedback = true;

    // Current state
    public int currentPosition;
    private int targetPosition;

    void Start()
    {
        currentPosition = centerPosition;
        targetPosition = centerPosition;
        
        if (beamRenderer == null)
        {
            beamRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        // Smoothly move to target position
        currentPosition = Mathf.RoundToInt(Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothSpeed));
        
        // Apply visual transformation
        if (useRotation)
        {
            // Rotate the beam (0 = left, 90 = center, 180 = right)
            float angle = currentPosition - 90; // Convert to -90 to +90 range
            transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            // Translate the beam horizontally
            float normalizedPos = (currentPosition - minPosition) / (float)(maxPosition - minPosition);
            float xPos = Mathf.Lerp(-2f, 2f, normalizedPos); // Adjust range as needed
            transform.localPosition = new Vector3(xPos, transform.localPosition.y, transform.localPosition.z);
        }

        // Color feedback
        if (useColorFeedback && beamRenderer != null)
        {
            bool isBalanced = Mathf.Abs(currentPosition - centerPosition) < 5; // Within 5 degrees
            beamRenderer.color = Color.Lerp(beamRenderer.color, 
                isBalanced ? balancedColor : unbalancedColor, 
                Time.deltaTime * 3f);
        }
    }

    /// <summary>
    /// Update the balance beam based on current charge
    /// </summary>
    /// <param name="currentCharge">Current total charge in beaker</param>
    /// <param name="targetCharge">Target charge for balance (usually 0)</param>
    public void UpdateBalance(int currentCharge, int targetCharge)
    {
        // Calculate deviation from target
        int chargeDeviation = currentCharge - targetCharge;
        
        // Map charge to position (0-180)
        // Negative charge -> left (0), Positive charge -> right (180), Zero -> center (90)
        float normalizedCharge = (float)chargeDeviation / chargeRange;
        normalizedCharge = Mathf.Clamp(normalizedCharge, -1f, 1f);
        
        targetPosition = centerPosition + Mathf.RoundToInt(normalizedCharge * (maxPosition - centerPosition));
        targetPosition = Mathf.Clamp(targetPosition, minPosition, maxPosition);
    }

    /// <summary>
    /// Get whether the beam is currently balanced
    /// </summary>
    public bool IsBalanced(int tolerance = 5)
    {
        return Mathf.Abs(currentPosition - centerPosition) <= tolerance;
    }
}


using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitrationDisplay : MonoBehaviour
{
    private TitrationManagerRedux titrationManager;
    private Compound compoundManager;
    private Compound.CompoundData currentCompound;
    private bool wasBalanced = false;
    private float balancedTime = 0f;
    private const float WIN_TIME = 2f; // Need 2 seconds balanced to win
    
    public TextMeshProUGUI compoundDisplayText; // Assign in Inspector
    public TextMeshProUGUI leftChargeText; // Assign in Inspector
    public TextMeshProUGUI rightChargeText; // Assign in Inspector


    private void Start()
    {
        titrationManager = FindObjectOfType<TitrationManagerRedux>();
        
        compoundManager = Compound.instance;
        Debug.Log($"Found Compound manager: {compoundManager != null}");

        if (compoundManager != null)
        {
            currentCompound = compoundManager.GetRandomCompound();
            Debug.Log($"Goal: balance {currentCompound.name} ({currentCompound.formula})");
            Debug.Log($"Target charge: {currentCompound.targetCharge}");
            
            if (compoundDisplayText != null)
            {
                compoundDisplayText.text = currentCompound.name;
                Debug.Log($"Display text set to: {compoundDisplayText.text}");
            }
            else
            {
                Debug.LogError("compoundDisplayText is not assigned!");
            }
            
            if (titrationManager != null)
            {
                titrationManager.targetCharge = currentCompound.targetCharge;
                titrationManager.PrintChargeInfo();
            }
        }
        else
        {
            Debug.LogError("Compound manager not found in scene!");
        }
    }

    private void Update()
    {
        if (titrationManager == null)
            return;

        bool isBalanced = titrationManager.IsBalanced();
        int leftCharge = titrationManager.GetLeftCharge();
        
        if (leftChargeText != null)
        {
            leftChargeText.text = $"Left: {leftCharge}";
        }
        
        if (rightChargeText != null)
        {
            rightChargeText.text = $"Target: {titrationManager.targetCharge}";
        }
        
        bool canWin = leftCharge > 0 && isBalanced;

        if (canWin)
        {
            balancedTime += Time.deltaTime;

            if (balancedTime >= WIN_TIME && !wasBalanced)
            {
                Debug.Log($"VICTORY! You successfully balanced {currentCompound.name}! ✓✓✓");
                wasBalanced = true;
            }
        }
        else
        {
            if (balancedTime > 0)
            {
                Debug.Log("Out of balance");
                balancedTime = 0f;
                wasBalanced = false;
            }
        }
    }
}


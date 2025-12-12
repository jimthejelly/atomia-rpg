using UnityEngine;

public class TitrationDisplay : MonoBehaviour
{
    private TitrationManagerRedux titrationManager;
    private Compound compoundManager;
    private Compound.CompoundData currentCompound;
    private bool wasBalanced = false;
    private float balancedTime = 0f;
    private const float WIN_TIME = 2f; // Need 2 seconds balanced to win

    private void Start()
    {
        titrationManager = FindObjectOfType<TitrationManagerRedux>();
        Debug.Log($"Found TitrationManagerRedux: {titrationManager != null}");
        
        compoundManager = FindObjectOfType<Compound>();
        Debug.Log($"Found Compound manager: {compoundManager != null}");

        // Load a random compound
        if (compoundManager != null)
        {
            currentCompound = compoundManager.GetRandomCompound();
            Debug.Log($"Goal: balance {currentCompound.name} ({currentCompound.formula})");
            Debug.Log($"Target charge: {currentCompound.targetCharge}");
            
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


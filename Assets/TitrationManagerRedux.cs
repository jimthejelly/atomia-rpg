using UnityEngine;

public class TitrationManagerRedux : MonoBehaviour
{
    public BalanceBeamTilt balanceBeam;
    
    [Tooltip("This represents the target charge for the user to receive")]
    public int targetCharge = 0;

    // charges for the left and right sides of the beam
    private int leftSideCharge = 0;
    private int rightSideCharge = 0;

    private void Start()
    {
        if (balanceBeam == null)
        {
            balanceBeam = GetComponent<BalanceBeamTilt>();
        }

        // disable  old TitrationManager
        TitrationManager oldManager = GetComponent<TitrationManager>();
        if (oldManager != null)
        {
            oldManager.enabled = false;
        }
    }

    public void AddElementLeft(string elementSymbol)
    {
        Weighting.Element element = Weighting.instance.GetElement(elementSymbol);
        if (element != null)
        {
            leftSideCharge += element.charge;
            UpdateBeam();
            Debug.Log($"added {element.name} to left. Left: {leftSideCharge}");
        }
    }

    public void AddElementRight(string elementSymbol)
    {
        Weighting.Element element = Weighting.instance.GetElement(elementSymbol);
        if (element != null)
        {
            rightSideCharge += element.charge;
            UpdateBeam();
            Debug.Log($"added {element.name} to right. Right: {rightSideCharge}");
        }
    }

    private void UpdateBeam()
    {
        int totalChargeDifference = leftSideCharge - rightSideCharge;
        balanceBeam.UpdateBalance(totalChargeDifference, targetCharge);
    }

    // checks for balancing on the balance beam
    public bool IsBalanced(int tolerance = 5)
    {
        return balanceBeam.IsBalanced(tolerance);
    }

    public int GetChargeDifference()
    {
        return leftSideCharge - rightSideCharge;
    }

    public void PrintChargeInfo()
    {
        int diff = leftSideCharge - rightSideCharge;
        Debug.Log($"Left: {leftSideCharge} | Right: {rightSideCharge} | Difference: {diff}");
    }

    public void Reset()
    {
        leftSideCharge = 0;
        rightSideCharge = 0;
        UpdateBeam();
        Debug.Log("titration reset");
    }

    public int GetLeftCharge()
    {
        return leftSideCharge;
    }

    public int GetRightCharge()
    {
        return rightSideCharge;
    }
}

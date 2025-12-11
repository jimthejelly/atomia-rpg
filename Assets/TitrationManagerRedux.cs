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
}

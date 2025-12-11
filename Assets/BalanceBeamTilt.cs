
using UnityEngine;

/* This script manages the behavior of the balance beam moving when the titration is
 * off-balance
 *
 *
*/


public class BalanceBeamTilt : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Balance Settings")]
    [Tooltip("Furthest left movement (negative charge)")]
    public int minPosition = 0;
    
    [Tooltip("Furthest right movement (negative charge)")]
    public int maxPosition = 180;
    
    [Tooltip("balanced position")]
    public int centerPosition = 90;
    
    [Tooltip("How many charge units to shift full scale")]
    public int chargeRange = 10;

    /* I'm thinking that the furthest left the position could be
        is 0, and the furthest right would be 180
        
        The goal would be to steadily have the position at 90
        (for a certain amount of time to be certain that the titration
        is good
    */
    [Header("Visual Settings")]
    [Tooltip("The beam should rotate")]
    public bool useRotation = true;
    
    [Tooltip("Smoothing speed for movement")]
    public float smoothSpeed = 5f;

    [Header("Color Feedback")]
    public SpriteRenderer beamRenderer;
    public Color balancedColor = Color.green;
    public Color unbalancedColor = Color.red;
    public bool useColorFeedback = true;

    public string[] leftChemical;
    public string[] rightChemical;
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

    // Update is called once per frame
    void Update()
    {
        currentPosition = Mathf.RoundToInt(Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothSpeed));
        
        // visual transformation
        if (useRotation){
            float angle = currentPosition - 90; // Convert to -90 to +90 range
            transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            // move the beam to match the state
            float normalizedPos = (currentPosition - minPosition) / (float)(maxPosition - minPosition);
            float xPos = Mathf.Lerp(-2f, 2f, normalizedPos);
            transform.localPosition = new Vector3(xPos, transform.localPosition.y, transform.localPosition.z);
        }

    }

    /* The function UpdateBalance serves to calculate the current weights of each side
    * of the balance beam to send a message. This will eventually result in the 
    * moving of the balance beam
    */
    public void UpdateBalance(int currentCharge, int targetCharge)
    {
        // calc difference from target
        int chargeDiff = currentCharge - targetCharge;
        
        // map charge to position (0-180)
        // ngative charge = left (0) positive charge = right (180) 0 -> center (90)
        float normCharge = (float) chargeDiff / chargeRange;
        normCharge = Mathf.Clamp(normCharge, -1f, 1f);

        targetPosition = centerPosition + Mathf.RoundToInt(normCharge * (maxPosition - centerPosition));
        targetPosition = Mathf.Clamp(targetPosition, minPosition, maxPosition);
    }

    /*
    * This function serves to check whether the balance beam is balanced or not
    */
    public bool IsBalanced(int tolerance = 5)
    {
        return Mathf.Abs(currentPosition - centerPosition) <= tolerance;
    }
}
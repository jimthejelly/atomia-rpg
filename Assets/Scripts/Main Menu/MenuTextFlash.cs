using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuTextFlash : MonoBehaviour
{
    public TMP_Text flashText;
    public Color colorA = Color.white;
    public Color colorB = Color.yellow;
    public float pulseSpeed = 1f;

    private void Update()
    {
        if (flashText != null)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // smoothly oscillates between 0 and 1
            flashText.color = Color.Lerp(colorA, colorB, t);
        }
    }
}
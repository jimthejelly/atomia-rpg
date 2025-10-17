using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void EndTurn()
    {
        GameManager.Instance.swapTurn();
    }

    public void DoMove(string move)
    {
        if (GameManager.Instance.minigametimeRemaining > 0f)
        {
            return;
        }
        if (move == "co2")
        {
            StartCoroutine(GameManager.Instance.MoveCO2());
        }
    }
}

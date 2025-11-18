using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void EndTurn()
    {
        if (GameManager.Instance.playerTurn)
        {
            GameManager.Instance.swapTurn();
        }
        
    }

    public void DoMove(string move)
    {
        if (GameManager.Instance.minigametimeRemaining > 0f)
        {
            return;
        }
        if (move == "co2")
        {
            Debug.Log("co2 button pressed");
            StartCoroutine(GameManager.Instance.MoveCO2());
            return;
        }
        if (move == "co")
        {
            StartCoroutine(GameManager.Instance.MoveCO());
        }
    }

}

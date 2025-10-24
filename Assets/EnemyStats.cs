using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int HP;
    public int HPmax;
    public string enemyName;

    public void DoMove()
    {
        GameManager.Instance.changePlayerHealth(
            Random.Range(1, GameManager.Instance.partyHealth.Count),
            GameManager.Instance.calculateTotalEnemyDamage(7f)
            );
    }
}

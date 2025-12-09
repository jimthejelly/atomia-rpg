using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBase
{

    public BasicEnemy()
    {
        hp = 40f;
        maxHp = 40f;
        enemyName = "Basic Enemy";

    }
    public override void DoMove()
    {
        base.DoMove();
        Debug.Log("muahahaha!");
    }
}

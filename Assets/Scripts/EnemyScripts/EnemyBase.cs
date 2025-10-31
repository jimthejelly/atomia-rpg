using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public string enemyName;

    public virtual void DoMove()
    {
        GameObject target = GameManager.Instance.getRandomPartyMember();
        if (target.GetComponent<PlayerBase>() == null)
        {
            Debug.Log("non-player in party!");
            return;
        } else
        {
            target.GetComponent<PlayerBase>().changePlayerHP(5f);
        }
    }

    public void changeEnemyHealth(float amt)
    {
        hp += amt;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(enemyName + " has died!");
        Destroy(this);
    }
}

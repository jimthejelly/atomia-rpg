using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public string enemyName;

    private GameObject healthBar;

    void Awake()
    {
        healthBar = transform.GetChild(0).gameObject;
    }

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
        Transform hb = healthBar.transform.GetChild(1);
        Vector3 hbScale = hb.localScale;
        Vector3 hbPos = hb.position;
        hbScale.x = hp / maxHp;
        hbPos.x -=  (1 - (hp / maxHp))/2;
        hb.position = hbPos;
        hb.localScale = hbScale;
        if (hp <= 0)
        {
            Die();
        }
    }

    public string GetName()
    {
        return enemyName;
    }

    private void Die()
    {
        Debug.Log(enemyName + " has died!");
        Destroy(this);
    }
}

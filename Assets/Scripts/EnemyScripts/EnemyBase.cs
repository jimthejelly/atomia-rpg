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
            float dmg = GameManager.Instance.calculateTotalEnemyDamage(25f);
            target.GetComponent<PlayerBase>().changePlayerHP(-dmg);
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
        Vector3 hbPos = hb.localPosition;
        hbScale.x = hp / maxHp;
        hbPos.x = -0.5f + (hbScale.x/2);
        hb.localPosition = hbPos;
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
        GameManager.Instance.removeEnemy(gameObject);
        Destroy(healthBar);
        Destroy(this.gameObject);
    }
}

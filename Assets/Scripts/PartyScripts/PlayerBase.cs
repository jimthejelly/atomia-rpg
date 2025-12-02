using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public string element;
    public string charName;
    private GameObject healthBar;

    void Awake()
    {
        healthBar = transform.GetChild(0).gameObject;
    }
    public void changePlayerHP(float amt)
    {
        hp += amt;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        Debug.Log(charName + " new hp = " + hp);
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
        return charName;
    }

    private void Die()
    {
        Debug.Log(name + " has died!");
    }
}

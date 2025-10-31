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

    public void changePlayerHP(float amt)
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
        Debug.Log(name + " has died!");
    }
}

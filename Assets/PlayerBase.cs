using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public float hp = 100f;
    public float maxHp = 100f;
    public string element = "";
    public string name = "";

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

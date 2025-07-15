using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl : MonoBehaviour
{
    //honestly i might just scrap this whole script, seems easier to just code the whole thing into BattleMenuControl instead of having these two scripts talking to each other
    public int playerHP;
    public int playerHPmax;
    public int playerAT;

    public int enemyHP;
    public int enemyHPmax;

    public string enemyName = "Rock";

    public string[] items = { "Pie" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl : MonoBehaviour
{
    public int playerHP;
    public int playerHPmax;
    public int playerAT;

    public int enemyHP;
    public int enemyHPmax;

    public bool waitingForMenu = true;
    public int playerAction = 0;

    public string enemyName = "Rock";

    public string[] items = { "Pie" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingForMenu == false)
        {
            
        }
    }
}

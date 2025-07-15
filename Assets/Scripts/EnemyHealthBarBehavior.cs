using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarBehavior : MonoBehaviour
{
    public GameObject BattleController;

    private float maxHP;
    private float HP;
    private float scale;

    public GameObject insideBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //update HP and maxHP with values from BattleControl
        maxHP = BattleController.GetComponent<BattleMenuControl>().enemyHPmax;
        HP = BattleController.GetComponent<BattleMenuControl>().enemyHP;

        //resize the HP bar
        scale = HP / maxHP;
        insideBar.transform.localScale = new Vector3(scale, 1, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BattleMenuControl : MonoBehaviour
{
    public GameObject BattleController;

    public int menuIndex = 0;
    private int menuSize = 4;
    public int menuLevel = 0;

    public GameObject TargetSelect;
    public TMP_Text Option1Text;

    //public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        TargetSelect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z")) //select
        {
            if (menuLevel == 0)
            {
                menuLevel = 1;
                TargetSelect.SetActive(true);
                if (menuIndex == 0) //"Fight" selected
                {
                    Option1Text.text = BattleController.GetComponent<BattleControl>().enemyName;
                }
                else if (menuIndex == 1) //"Act" selected
                {
                    Option1Text.text = "Check";
                }
                else if (menuIndex == 2) //"Item" selected
                {
                    Option1Text.text = BattleController.GetComponent<BattleControl>().items[0];
                }
                else if (menuIndex == 3) //"Run" selected
                {
                    Option1Text.text = "i'll implement running later";
                }
            }
        }
        if (Input.GetKeyDown("x")) //back
        {
            if (menuLevel == 1)
            {
                menuLevel = 0;
                TargetSelect.SetActive(false);
            }
        }
        if (Input.GetKeyDown("up")) //scroll to previous
        {
            if (menuLevel == 0) 
            {
                menuIndex--;
            }
        }
        if (Input.GetKeyDown("down")) //scroll to next
        {
            if (menuLevel == 0)
            {
                menuIndex++;
            }
        }

        //reset menuIndex if out of bounds
        if (menuIndex < 0)
        {
            menuIndex = menuSize-1;
        }
        if (menuIndex >= menuSize)
        {
            menuIndex = 0;
        }

        //set menu sprite
        GetComponent<SpriteRenderer>().sprite = sprites[menuIndex];
    }
}


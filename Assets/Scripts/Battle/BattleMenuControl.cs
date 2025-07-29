using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BattleMenuControl : MonoBehaviour
{
    //public GameObject BattleController;

    private int menuIndex = 0;
    private int menuLevel = 0;
    private static int menuSize = 4;

    public GameObject ActionSelect;
    public GameObject TargetSelect;
    public GameObject Cursor;
    public GameObject BattleBox;
    public GameObject PlayerDot;
    public GameObject BulletsController;

    public TMP_Text DescText;
    public TMP_Text Option1Text;

    public int playerHP = 50;
    public int playerHPmax = 50;
    public int playerAT = 5;

    public int enemyHP = 20;
    public int enemyHPmax = 20;

    public string enemyName = "Rock";

    public string[] items = { "Pie" };

    public Sprite[] menusprites;

    bool bulletsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        TargetSelect.SetActive(false);
        BattleBox.SetActive(false);
        PlayerDot.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //MENU STUFF
        if (Input.GetKeyDown("z")) //select
        {
            if (menuLevel == 0)
            {
                menuLevel = 1;
                TargetSelect.SetActive(true);
                if (menuIndex == 0) //"Fight" selected
                {
                    Option1Text.text = enemyName;
                }
                else if (menuIndex == 1) //"Act" selected
                {
                    Option1Text.text = "Check";
                }
                else if (menuIndex == 2) //"Item" selected
                {
                    Option1Text.text = items[0];
                }
                else if (menuIndex == 3) //"Run" selected
                {
                    Option1Text.text = "i'll implement running later";
                }
            }
            else if (menuLevel == 1)
            {
                if (menuIndex == 0) //"Fight" selected
                {
                    TargetSelect.SetActive(false);
                    enemyHP -= playerAT;
                    //menuLevel = -1;
                    ActionSelect.SetActive(false);
                    EnableBullets();
                }
                else if (menuIndex == 1) //"Act" selected
                {
                    Option1Text.text = "";
                    DescText.text = "Rock: 0 AT, 0 DF\nWeak placeholder enemy";
                    //menuLevel = -1;
                    Cursor.SetActive(false);
                }
                else if (menuIndex == 2) //"Item" selected
                {
                    Option1Text.text = items[0];
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
        ActionSelect.GetComponent<SpriteRenderer>().sprite = menusprites[menuIndex];



        //BULLET STUFF
        if(bulletsEnabled && BulletsController.GetComponent<BulletsTest>().inprogress == false)
        {
            DisableBullets();
        }
    }

    void EnableBullets()
    {
        BattleBox.SetActive(true);
        PlayerDot.SetActive(true);

        BulletsController.GetComponent<BulletsTest>().inprogress = true;
        PlayerDot.GetComponent<BattleMovement>().canMove = true;

        bulletsEnabled = true;
    }

    void DisableBullets()
    {
        BattleBox.SetActive(false);
        PlayerDot.SetActive(false);

        PlayerDot.GetComponent<BattleMovement>().canMove = false;

        ActionSelect.SetActive(true);
        menuLevel = 0;
        menuIndex = 0;

        bulletsEnabled = false;
    }
}


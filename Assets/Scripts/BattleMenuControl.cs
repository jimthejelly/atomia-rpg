using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenuControl : MonoBehaviour
{
    public int menuIndex = 0;
    private int menuSize = 4;

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z")) //select
        {
           
        }
        if (Input.GetKeyDown("up")) //scroll to previous
        {
            menuIndex--;
        }
        if (Input.GetKeyDown("down")) //scroll to next
        {
            menuIndex++;
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
        spriteRenderer.sprite = sprites[menuIndex];
    }
}

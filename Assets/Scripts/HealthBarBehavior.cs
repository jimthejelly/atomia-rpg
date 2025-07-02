using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBarBehavior : MonoBehaviour
{
    public float maxHP;
    public float HP;
    private float scale;

    public GameObject insideBar;
    public TMP_Text HP_text;
    public TMP_Text maxHP_text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scale = HP / maxHP;
        insideBar.transform.localScale = new Vector3(scale, 1, 1);
        HP_text.text = HP.ToString("0");
        maxHP_text.text = maxHP.ToString("0");
    }
}

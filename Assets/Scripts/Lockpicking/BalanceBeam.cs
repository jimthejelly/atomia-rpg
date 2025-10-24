using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceBeam : MonoBehaviour
{
    // Start is called before the first frame update

    public int currentPostion;
    /* I'm thinking that the furthest left the position could be
        is 0, and the furthest right would be 180
        
        The goal would be to steadily have the position at 90
        (for a certain amount of time to be certain that the titration
        is good
    */

    public string[] leftChemical;
    public string[] rightChemical;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStop : MonoBehaviour
{
    public Initial_Chase chase_scipt;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            chase_scipt.chaseOn = false;
        }
    }
}
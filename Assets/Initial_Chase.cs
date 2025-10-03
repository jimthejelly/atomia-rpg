using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initial_Chase : MonoBehaviour
{
    public float speed = 3f;
    private Transform target;
    public bool chaseOn = false;

    private void Update() {
        if(chaseOn && target!=null) {
            float step = speed*Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            target = other.transform;
            chaseOn = true;
            Debug.Log("Chase start!");
        }

        if (other.gameObject.tag == "ChaseStop") {
            chaseOn = false;
            Debug.Log("Stop chase triggered.");
        }
    }
}

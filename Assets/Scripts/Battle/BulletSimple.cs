using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSimple : MonoBehaviour
{
    public int dir; //0 up, 1 right, 2 down, 3 left
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dir == 0)
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        }
        else if (dir == 1)
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        if (dir == 2)
        {
            transform.position += new Vector3(0, -1*speed, 0) * Time.deltaTime;
        }
        else if (dir == 3)
        {
            transform.position += new Vector3(-1*speed, 0, 0) * Time.deltaTime;
        }

        //destroy when out of bounds
        if (transform.position.x < -3 || transform.position.x > 3 || transform.position.y < -3 || transform.position.y > 3) {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Text: colliding!");
        Destroy(gameObject);
    }
}

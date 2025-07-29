using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMovement : MonoBehaviour
{
    private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 movement;

    public bool canMove = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            // Get input from player
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Move the character
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}

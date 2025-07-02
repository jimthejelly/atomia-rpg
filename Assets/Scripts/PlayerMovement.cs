using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public Rigidbody2D rb;

    private Vector2 change;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", change.x);
        animator.SetFloat("Vertical", change.y);

        float isMoving = (change != Vector2.zero) ? 1f : 0f;
        animator.SetFloat("Speed", change.sqrMagnitude);
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + change * speed * Time.fixedDeltaTime);
    }
}
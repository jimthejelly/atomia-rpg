using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable{get; set;}

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
        if(dialogueUI.IsOpen) {
            change = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", change.x);
        animator.SetFloat("Vertical", change.y);

        float isMoving = (change != Vector2.zero) ? 1f : 0f;
        animator.SetFloat("Speed", change.sqrMagnitude);

        if(Input.GetKeyDown(KeyCode.Z)) {
            if(Interactable != null) {
                Interactable.Interact(this);
            }
        }
    }

    void FixedUpdate() {
        if (dialogueUI.IsOpen) {
            return;
        }

        rb.MovePosition(rb.position + change * speed * Time.fixedDeltaTime);
    }
}
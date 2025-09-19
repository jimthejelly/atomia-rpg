using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    public Animator animator;
    public float speed;
    public float oldLastDir = 0;

    public Rigidbody2D rb;
    private Vector2 change;

    private float lastDir(float x, float y, float oldLastDir)
    {
        // 0 down, 0.3 up, 0.7 left, 1 right
        if (x == 0)
        {
            if (y == -1) // down
            {
                return 0;
            }
            if (y == 0)
            {
                return oldLastDir;
            }
            if (y == 1) // up
            {
                return 0.3f;
            }
        }
        if (x == -1) // left
        {
            if (y == -1)
            {
                return 0.7f;
            }
            if (y == 0)
            {
                return 0.7f;
            }
            if (y == 1)
            {
                return 0.3f;
            }
        }
        if (x == 1) // right
        {
            if (y == -1)
            {
                return 1f;
            }
            if (y == 0)
            {
                return 1f;
            }
            if (y == 1)
            {
                return 0.3f;
            }
        }
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueUI.IsOpen)
        {
            change = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", change.x);
        animator.SetFloat("Vertical", change.y);
        oldLastDir = lastDir(change.x, change.y, oldLastDir);
        animator.SetFloat("LastDir", oldLastDir);

        float isMoving = (change != Vector2.zero) ? 1f : 0f;
        animator.SetFloat("Speed", change.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
    }

    void FixedUpdate()
    {
        if (dialogueUI.IsOpen)
        {
            return;
        }

        rb.MovePosition(rb.position + change.normalized * speed * Time.fixedDeltaTime);
    }
}
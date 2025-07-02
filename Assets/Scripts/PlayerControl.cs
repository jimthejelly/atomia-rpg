using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) { 
            // Get input from player
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Move the character
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }   

        if(Input.GetKeyDown("z"))
        {
            canMove = false;
            SceneManager.LoadScene("KaiBattleTestZone", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("KaiBattleTestZone"));
        }
    }
}

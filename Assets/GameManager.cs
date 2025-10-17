using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool playerTurn = false;
    public bool enemyMoving = false;
    [SerializeField] private GameObject playerButtons;

    [SerializeField] private GameObject player, enemy, turnIndicator;
    [SerializeField] private TMP_Text cText, oText;
    [SerializeField] private GameObject minigame, goalcircle, movingcircle;

    private float minigametimeRemaining;
    private float minigameMaxTime = 3f;
    private float damageMult = 1f;

    public int c, o = 0;


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Optional: Prevent the GameObject from being destroyed when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        swapTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            swapTurn();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            c += 3;
            o += 3;
            updateElementText();
        }

        if (!playerTurn && !enemyMoving)
        {
            Debug.Log("enemy attacks!");
            Invoke("swapTurn", 1f);
            enemyMoving = true;
            
        }

        if (minigametimeRemaining > 0f)
        {
            float desiredSize = goalcircle.transform.localScale.x;
            minigametimeRemaining -= Time.deltaTime;
            movingcircle.transform.localScale *= 0.995f;
        } else if (minigametimeRemaining <= 0f)
        {
            minigametimeRemaining = 0f;
            movingcircle.transform.localScale = new Vector3(2f, 2f, 2f);
            minigame.SetActive(false);
        }
        
        if (minigametimeRemaining > 0f && Input.GetKeyDown(KeyCode.F))
        {
            float desiredSize = goalcircle.transform.localScale.x;
            float realSize = movingcircle.transform.localScale.x;
            if (Math.Abs(desiredSize - realSize) < 0.2f * desiredSize)
            {
                Debug.Log("Success!");
                damageMult = 1.5f;
                // minigametimeRemaining = 0f;
            }
            else
            {
                Debug.Log("Fail!");
                // minigametimeRemaining = 0f;
            }
            minigametimeRemaining = 0f;
        }
    }

    public void swapTurn()
    {
        playerTurn = !playerTurn;
        if (playerTurn) // state during player turn
        {
            c += 2;
            o += 2;

            enemyMoving = false;

            Vector3 trianglepos = player.transform.position;
            trianglepos.y += 1.5f;
            turnIndicator.transform.position = trianglepos;
        }
        else // state during enemy turn
        {
            Vector3 trianglepos = enemy.transform.position;
            trianglepos.y += 1.5f;
            turnIndicator.transform.position = trianglepos;
        }
    }

    private void updateElementText()
    {
        cText.text = "C: " + c;
        oText.text = "O: " + o;
    }

    private void startMinigame()
    {
        minigametimeRemaining = minigameMaxTime;
        minigame.SetActive(true);
    }
    
    public IEnumerator MoveCO2()
    {
        if (c < 1 && o < 2)
        {
            Debug.Log("not enough atoms!");
            yield break;
        }
        c -= 1;
        o -= 2;
        startMinigame();
        yield return new WaitUntil(() => minigametimeRemaining <= 0);
        Debug.Log("did " + 5 * damageMult + " damage!");
        damageMult = 1f;
    }
}

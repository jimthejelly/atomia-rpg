using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool playerTurn = true;
    public bool enemyMoving = false;
    [SerializeField] private GameObject playerButtons;

    [SerializeField] private GameObject player, enemy, turnIndicator;
    [SerializeField] private TMP_Text cText, oText, minigamePromptText;
    [SerializeField] private GameObject minigame, goalcircle, movingcircle;

    public float minigametimeRemaining;
    private float minigameMaxTime = 1f;
    private float damageMult = 1f;

    public int c, o = 0;
    private Color movingCircleColor = new Color(160, 81, 255, 75);


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
        addElement("c", 2);
        addElement("o", 2);
    }

    public void addElement(string element, int num)
    {
        if (element == "c")
        {
            c += num;
            cText.text = "C: " + c;
            return;
        }
        if (element == "o")
        {
            o += num;
            oText.text = "O: " + o;
        }
        else
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            addElement("c", 3);
            addElement("o", 3);
        }

        if (!playerTurn && !enemyMoving)
        {
            StartCoroutine(EnemyTurn());
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
            minigamePromptText.gameObject.SetActive(false);
        }
        
        if (minigametimeRemaining > 0f && Input.GetKeyDown(KeyCode.Space))
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
        minigamePromptText.gameObject.SetActive(true);
    }

    public IEnumerator MoveCO2()
    {
        if (c < 1 || o < 2)
        {
            Debug.Log("not enough atoms!");
            yield break;
        }
        addElement("c", -1);
        addElement("o", -2);
        startMinigame();
        yield return new WaitUntil(() => minigametimeRemaining <= 0);
        Debug.Log("did " + 5 * damageMult + " damage!");
        damageMult = 1f;
    }

    public IEnumerator MoveCO()
    {
        yield return new WaitForSeconds(1f);
    }
    
    public IEnumerator EnemyTurn() 
    {
        if (playerTurn)
        {
            Debug.Log("enemy cant attack during your turn!");
            yield break;
        }
        Debug.Log("enemy attacks!");
        enemyMoving = true;
        yield return new WaitForSeconds(1.5f);
        enemyMoving = false;
        swapTurn();
    }
}

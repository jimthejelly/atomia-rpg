using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool playerTurn = true;
    public bool enemyMoving = false;
    public int coDebuffTurns = 0;
    private string currentMove = "";
    public int partySize = 1;
    [SerializeField] private GameObject playerButtons;

    [SerializeField] private GameObject player, enemy, turnIndicator, debuffIcon;
    [SerializeField] private TMP_Text cText, oText, minigamePromptText, partyHealthText;
    [SerializeField] private GameObject minigame, goalcircle, movingcircle;

    public float minigametimeRemaining;
    private float minigameMaxTime = 1.2f;
    private Dictionary<string, float> playerDamageMults = new Dictionary<string, float>();
    private Dictionary<string, float> enemyDamageMults = new Dictionary<string, float>();

    public Dictionary<string, GameObject> party;
    public Dictionary<string, GameObject> enemies;

    // only for adding characters from the inspector
    [SerializeField] private List<GameObject> allCharacters;
    [SerializeField] private List<GameObject> allEnemies;

    // should be used when loading a party
    private Dictionary<string, GameObject> allCharactersDict;    
    private Dictionary<string, GameObject> allEnemiesDict;

    public int c, o = 0;


    private void Awake()
    {
        // converts (serializable) lists into (unserializable but easily searchable) dictionaries
        foreach (GameObject character in allCharacters)
        { // adds all characters from allCharacters into a dictionary
            if (character.GetComponent<PlayerBase>() != null)
            {
                allCharactersDict.Add(character.GetComponent<PlayerBase>().charName, character);
            }
        }
        foreach (GameObject enemy in allEnemies)
        { // adds all enemies from allEnemies into a dictionary
            if (enemy.GetComponent<EnemyBase>() != null)
            {
                allEnemiesDict.Add(enemy.GetComponent<EnemyBase>().enemyName, enemy);
            }
        }
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
        addElementsFromParty();
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

    private void addElementsFromParty() // adds elements based on what members are in your party
    {
        if (party.ContainsKey("Arpie"))
        {
            addElement("c", 2);
        }
        if (party.ContainsKey("Oxie"))
        {
            addElement("o", 2);
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

        if (minigametimeRemaining > 0f) // reduce the size of the moving circle
        {
            float desiredSize = goalcircle.transform.localScale.x;
            minigametimeRemaining -= Time.deltaTime;
            movingcircle.transform.localScale *= 0.995f;
        } else if (minigametimeRemaining <= 0f) // end minigame when time is up
        {

            minigametimeRemaining = 0f;
            movingcircle.transform.localScale = new Vector3(2f, 2f, 2f);
            minigame.SetActive(false);
            minigamePromptText.gameObject.SetActive(false);
        }

        if (minigametimeRemaining > 0f && Input.GetKeyDown(KeyCode.Space)) // check if player won the minigame
        {
            float desiredSize = goalcircle.transform.localScale.x;
            float realSize = movingcircle.transform.localScale.x;
            if (Math.Abs(desiredSize - realSize) < 0.2f * desiredSize)
            {
                Debug.Log("Success!");
                doMinigameEffect(currentMove);
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

    public GameObject getRandomPartyMember()
    {
        List<GameObject> members = Enumerable.ToList(party.Values);
        return members[UnityEngine.Random.Range(0, members.Count)];
    }

    public void swapTurn()
    {
        playerTurn = !playerTurn;
        if (playerTurn) // state during player turn
        {
            addElement("c", 2);
            addElement("o", 2);

            enemyMoving = false;
            decrementDebuffs();

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

    private void doMinigameEffect(string move) // applies positive effect of winning the minigame
    {
        if (move == "co2")
        {
            playerDamageMults.Add("co2", 1.5f);
        }
        else if (move == "co")
        {
            coDebuffTurns += 1;
        }
        else
        {
            Debug.Log("Move " + move + " does not exist!");
        }
    }

    public void decrementDebuffs() // decrements debuff timers and removes effects after they're done
    {
        if (coDebuffTurns > 0)
        {
            coDebuffTurns--;
            if (coDebuffTurns == 0)
            {
                enemyDamageMults.Remove("co");
            }
        }
        if (enemyDamageMults.Count == 0)
        {
            debuffIcon.SetActive(false);
        }
    }

    private float calculateTotalPlayerDamage(float baseDamage) // calculates total player damage
    {
        foreach (float mult in playerDamageMults.Values)
        {
            baseDamage *= mult;
        }
        return baseDamage;
    }

    public float calculateTotalEnemyDamage(float baseDamage) // calculates total enemy damage
    {
        foreach (float mult in enemyDamageMults.Values)
        {
            baseDamage *= mult;
        }
        return baseDamage;
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
        currentMove = "co2";
        startMinigame();
        yield return new WaitUntil(() => minigametimeRemaining <= 0);
        Debug.Log("did " + calculateTotalPlayerDamage(5) + " damage!");
        playerDamageMults.Remove("co2");
        currentMove = "";
    }

    public IEnumerator MoveCO()
    {
        if (c < 2 || o < 2)
        {
            Debug.Log("not enough atoms!");
            yield break;
        }
        addElement("c", -2);
        addElement("o", -2);
        currentMove = "co";
        startMinigame();
        yield return new WaitUntil(() => minigametimeRemaining <= 0);
        coDebuffTurns += 2;
        Debug.Log("debuffed enemies for " + coDebuffTurns + " turns!");
        enemyDamageMults.Add("co", 0.85f);
        debuffIcon.SetActive(true);
        currentMove = "";
    }

    public IEnumerator EnemyTurn()
    {
        if (playerTurn)
        {
            Debug.Log("enemy cant attack during your turn!");
            yield break;
        }
        Debug.Log("enemy attacks for " + calculateTotalEnemyDamage(7) + " damage!");
        enemyMoving = true;
        yield return new WaitForSeconds(1.5f);
        enemyMoving = false;
        swapTurn();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool playerTurn = true;
    public bool enemyMoving = false;
    public bool choosingTarget = false;
    public int chosenTarget = 0;
    public int coDebuffTurns = 0;
    private string currentMove = "";
    [SerializeField] private GameObject playerButtons;

    [SerializeField] private GameObject turnIndicator, targetIndicator, debuffIcon;
    [SerializeField] private TMP_Text cText, oText, minigamePromptText, partyHealthText;
    [SerializeField] private GameObject minigame, goalcircle, movingcircle;

    public float minigametimeRemaining;
    private float minigameMaxTime = 1.5f;
    private Dictionary<string, float> playerDamageMults = new Dictionary<string, float>();
    private Dictionary<string, float> enemyDamageMults = new Dictionary<string, float>();

    public Dictionary<string, GameObject> party = new Dictionary<string, GameObject>(); // dictionary because no duplicates
    public List<GameObject> enemies = new List<GameObject>(); // allows duplicates, must be a list

    // only for adding characters from the inspector
    [SerializeField] private List<GameObject> allCharacters;
    [SerializeField] private List<GameObject> allEnemies;

    // should be used when loading a party
    private Dictionary<string, GameObject> allCharactersDict = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> allEnemiesDict = new Dictionary<string, GameObject>();

    private Vector3[] partyPositions = // position of the (up to) 4 party members
    {
        new Vector3(-3f, -0.5f, 0f),
        new Vector3(-4f, 0.25f, 0f),
        new Vector3(-5f, -0.5f, 0f),
        new Vector3(-6f, 0.25f, 0f)
    };
    private Vector3[] enemyPositions = // position of the (up to) 4 enemies
    {
        new Vector3(3f, -0.5f, 0f),
        new Vector3(4f, 0.25f, 0f),
        new Vector3(5f, -0.5f, 0f),
        new Vector3(6f, 0.25f, 0f)
    };

    public int c, o = 0;


    private void Awake()
    {
        // converts (serializable) lists into (unserializable but easily searchable) dictionaries
        foreach (GameObject character in allCharacters)
        { // adds all characters from allCharacters into a dictionary
            if (character.GetComponent<PlayerBase>() != null)
            {
                allCharactersDict.Add(character.GetComponent<PlayerBase>().GetName(), character);
            }
        }
        foreach (GameObject enemy in allEnemies)
        { // adds all enemies from allEnemies into a dictionary
            if (enemy.GetComponent<EnemyBase>() != null)
            {
                allEnemiesDict.Add(enemy.GetComponent<EnemyBase>().GetName(), enemy);
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
        LoadCombat(new string[] { "Arpie", "Oxie" }, new string[] { "Basic Enemy", "Basic Enemy" });
        addElementsFromParty();
    }

    void Update()
    {
        // ---- debug stuff ----
        if (Input.GetKeyDown(KeyCode.C))
        {
            addElement("c", 3);
            addElement("o", 3);
        }

        // start enemy turn when player turn ends
        if (!playerTurn && !enemyMoving)
        {
            StartCoroutine(EnemyTurn());
        }

        // ---- minigame handling ----

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

        // ---- target selection handling ----
        
        if (choosingTarget && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("switching target left");
            chosenTarget--;
            if (chosenTarget < 0)
            {
                chosenTarget = enemies.Count - 1;
            }
            Vector3 trianglePos = enemies[chosenTarget].transform.position;
            trianglePos.y += 1.2f;
            targetIndicator.transform.position = trianglePos;
        }
        if (choosingTarget && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("switching target right");
            chosenTarget++;
            if (chosenTarget >= enemies.Count)
            {
                chosenTarget = 0;
            }
            Vector3 trianglePos = enemies[chosenTarget].transform.position;
            trianglePos.y += 1.2f;
            targetIndicator.transform.position = trianglePos;
        }
        if (choosingTarget && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("target chosen");
            choosingTarget = false;
        }
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

    public void LoadCombat(string[] partyToLoad, string[] enemiesToLoad) 
    // Syntax: two strings with the exact names of each of the required characters
    // e.g. {Arpie, Oxie, Henry}, {Basic Enemy, Boss Enemy}
    {
        for (int i = 0; i < partyToLoad.Length; i++) // load party members
        {
            string member = partyToLoad[i];
            if (allCharactersDict.ContainsKey(member)) // if they're in the dictionary
            {
                if (allCharactersDict.TryGetValue(member, out GameObject value))
                {
                    GameObject characterInWorld = Instantiate(value, partyPositions[i], Quaternion.identity); // instantiate them in the world
                    party.Add(characterInWorld.GetComponent<PlayerBase>().GetName(), characterInWorld); // add them to the party dictionary
                }

            }
            else // error catching
            {
                Debug.Log("Dictionary does not contain " + member + "!");
            }
        }
        for (int i = 0; i < enemiesToLoad.Length; i++) // load enemies
        {
            string member = enemiesToLoad[i];
            if (allEnemiesDict.ContainsKey(member))
            {
                if (allEnemiesDict.TryGetValue(member, out GameObject value))
                {
                    GameObject enemyInWorld = Instantiate(value, enemyPositions[i], Quaternion.identity); // instantiate them in the world
                    enemies.Add(enemyInWorld); // add them to the enemy party list
                }
            }
        }
        SetTurnIndicator();
    }

    public GameObject getRandomPartyMember()
    {
        List<GameObject> members = Enumerable.ToList(party.Values);
        return members[UnityEngine.Random.Range(0, members.Count)];
    }

    private void SetTurnIndicator() // sets the position of the turn indicator upon the switching of turns (individual enemy turn indicator is handled in EnemyTurn)
    {
        if (playerTurn)
        {
            int lastPartyMember = party.Count - 1;
            Vector3 trianglePos = Vector3.Lerp(partyPositions[0], partyPositions[lastPartyMember], 0.5f); // get the midpoint between first and last party members
            trianglePos.y += 2f;
            turnIndicator.transform.position = trianglePos;
        } else
        {
            // move turn indicator to first enemy
            Vector3 trianglepos = enemies[0].transform.position;
            trianglepos.y += 1.25f;
            turnIndicator.transform.position = trianglepos;
        }
    }

    public void swapTurn()
    {
        playerTurn = !playerTurn;
        if (playerTurn) // state during player turn
        {
            addElementsFromParty();

            enemyMoving = false;
            decrementDebuffs();

            SetTurnIndicator();

        }
        else // state during enemy turn
        {
            SetTurnIndicator();
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

    private void StartTargeting()
    {
        if (choosingTarget)
        {
            Debug.Log("targeting already in progress!");
            return;
        }
        choosingTarget = true;
        targetIndicator.SetActive(true);
        chosenTarget = 0;
        Vector3 trianglePos = enemies[chosenTarget].transform.position;
        trianglePos.y += 1.2f;
        targetIndicator.transform.position = trianglePos;
    }

    // ------------------------------------------ MOVES ------------------------------------------

    public IEnumerator MoveCO2()
    {
        Debug.Log("calling moveCO2");
        // check if you can even cast it
        if (c < 1 || o < 2)
        {
            Debug.Log("not enough atoms!");
            yield break;
        }

        // begin targeting sequence
        StartTargeting();
        yield return new WaitUntil(() => choosingTarget == false);
        Debug.Log("target chosen");

        // spend elements
        addElement("c", -1);
        addElement("o", -2);
        Debug.Log("c: " + c + " o: " + o);

        // start minigame
        currentMove = "co2";
        startMinigame();
        yield return new WaitUntil(() => minigametimeRemaining <= 0);

        // do actual move
        float dmg = calculateTotalPlayerDamage(5);
        enemies[chosenTarget].GetComponent<EnemyBase>().changeEnemyHealth(-dmg);
        Debug.Log("did " + dmg + " damage!");

        // clean up
        chosenTarget = 0;
        targetIndicator.SetActive(false);
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

    // ------------------------------------------ END OF MOVES ------------------------------------------

    public IEnumerator EnemyTurn()
    {
        if (playerTurn)
        {
            Debug.Log("enemy cant attack during your turn!");
            yield break;
        }
        enemyMoving = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 trianglePos = enemies[i].transform.position;
            trianglePos.y += 1.5f;
            turnIndicator.transform.position = trianglePos;
            enemies[i].GetComponent<EnemyBase>().DoMove();
            yield return new WaitForSeconds(1f);
        }
        enemyMoving = false;
        swapTurn();
    }
}

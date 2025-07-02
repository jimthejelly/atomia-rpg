using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed;

    private string[] dialogueLines;
    private int currentLineIndex;
    private bool isTyping;

    void Start()
    {
        string[] openingLines = new string[]
        {
            "Hmm.... not this mushroom..."
        };

        StartDialogue(openingLines);
    }

    public void StartDialogue(string[] lines)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
    }

    void Update()
    {
        if (dialoguePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Z))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < dialogueLines.Length)
                    StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
                else
                    EndDialogue();
            }
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    System.Collections.IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}

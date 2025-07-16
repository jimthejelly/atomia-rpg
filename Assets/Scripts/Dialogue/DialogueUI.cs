using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private DialogueObject test;

    private TypewriterEffect typewriterEffect;

    private void Start() {
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
        ShowDialogue(test);
    }

    public void ShowDialogue(DialogueObject dialogueObject) {
        DialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) {
        foreach (DialogueLine line in dialogueObject.DialogueLines) {
            nameLabel.text = line.speakerName;
            yield return typewriterEffect.Run(line.text, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        CloseDialogueBox();
    }

    private void CloseDialogueBox() {
        DialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}

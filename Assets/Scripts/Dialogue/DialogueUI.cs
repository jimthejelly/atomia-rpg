using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private DialogueObject test;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    private void Start() {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject) {
        IsOpen = true;
        DialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) {
        for(int i=0; i<dialogueObject.DialogueLines.Length; i++) {            
            DialogueLine line = dialogueObject.DialogueLines[i];
            nameLabel.text = line.speakerName;

            yield return RunTypingEffect(line.text);

            textLabel.text = line.text;

            if(i == dialogueObject.DialogueLines.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if(dialogueObject.HasResponses) {
            responseHandler.ShowResponses(dialogueObject.Responses, OnResponseSelected);
        } else {
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue) {
        typewriterEffect.Run(dialogue, textLabel);

        while(typewriterEffect.IsRunning) {
            yield return null;

            if(Input.GetKeyDown(KeyCode.X)) {
                typewriterEffect.Stop();
            }
        }
    }

    private void CloseDialogueBox() {
        IsOpen = false;
        DialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

    private void OnResponseSelected(Response selectedResponse) {
        ShowDialogue(selectedResponse.DialogueObject);
    }
}
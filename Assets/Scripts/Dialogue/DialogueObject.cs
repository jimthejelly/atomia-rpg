using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea] public string text;
}

public class DialogueObject : ScriptableObject
{
    [SerializeField] private DialogueLine[] dialogueLines;

    public DialogueLine[] DialogueLines => dialogueLines;
}

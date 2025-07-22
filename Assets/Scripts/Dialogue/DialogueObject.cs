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
    [SerializeField] private Response[] responses;

    public DialogueLine[] DialogueLines => dialogueLines;

    public bool HasResponses => Responses != null && Responses.Length > 0;

    public Response[] Responses => responses;
}

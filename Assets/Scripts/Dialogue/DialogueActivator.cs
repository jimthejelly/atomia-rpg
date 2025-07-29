using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool canRepeat = false;
    [SerializeField] private bool resetOnExit = false;

    private bool hasInteracted = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player)) {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player)) {
            if(player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this) {
                player.Interactable = null;
            }

            if (resetOnExit)
            {
                hasInteracted = false;
            }
        }
    }

    public void Interact(PlayerMovement player) {
        if (hasInteracted && !canRepeat) return;

        hasInteracted = true;
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}

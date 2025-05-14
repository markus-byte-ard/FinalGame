using UnityEngine;
using UnityEngine.EventSystems;

public class BurnZone : MonoBehaviour, IDropHandler {
     public AudioSource burnAudio = null;

    public void OnDrop(PointerEventData eventData) {
        GameObject obj = eventData.pointerDrag;
        Card card = obj.GetComponent<Card>();

        // Ensure it's the player's turn and the player is not dead
        if (card != null && GameController.instance.playersTurn && GameController.instance.player.health > 0) {
            PlayBurnSound();
            GameController.instance.playersHand.RemoveCard(card);
            if (GameController.instance.player.mana < 7) {
                GameController.instance.player.mana++;
            }
            GameController.instance.NextPlayersTurn();
        } else {
            // Debug messages
            if (!GameController.instance.playersTurn) {
                Debug.Log("It's not the player's turn!");
            }
            if (GameController.instance.player.health <= 0) {
                Debug.Log("Player is dead, can't burn cards.");
            }
        }
    }

    internal void PlayBurnSound() {
        burnAudio.Play();
    }

}
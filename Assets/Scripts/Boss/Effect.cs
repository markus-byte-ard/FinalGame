using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour {
    public Player targetPlayer = null;
    public Card sourceCard = null;
    public UnityEngine.UI.Image effectImage = null;

    public AudioSource iceAudio = null;
    public AudioSource fireballAudio = null;
    public AudioSource destroyAudio = null;

    public void EndTrigger() {
        bool bounce = false;
        if (targetPlayer.HasMirror()) {
            bounce = true;
            targetPlayer.SetMirror(false);
            targetPlayer.PlaySmashSound();
            if (targetPlayer.isPlayer) {
                GameController.instance.CastAttackEffect(sourceCard, GameController.instance.enemy);
            } else {
                GameController.instance.CastAttackEffect(sourceCard, GameController.instance.player);
            }
        } else {
            int damage = sourceCard.cardData.damage;

            targetPlayer.health -= damage;
            if(targetPlayer.health < 0) {
                targetPlayer.health = 0;
            }
            targetPlayer.PlayHitAnim();
            targetPlayer.PlayHitSound();

            GameController.instance.UpdateHealths();

            if (targetPlayer.health <= 0) {

                if (targetPlayer.isPlayer) {
                    GameController.instance.PlayPlayerDieSound();
                } else {
                    GameController.instance.PlayEnemyDieSound();
                }
            }

            // If the card is Ice, freeze the target for 1 turn
            if (sourceCard.cardData.damageType == CardData.DamageType.Ice || sourceCard.cardData.damageType == CardData.DamageType.Both) {
                targetPlayer.Freeze(1);  // Freeze the target for 1 turn
            }

            if(!bounce) {
                GameController.instance.NextPlayersTurn();
            }
            GameController.instance.cardIsPlayable = true;
        }
        Destroy(gameObject);
    }

    internal void PlayIceSound() {
        iceAudio.Play();
    }

    internal void PlayFireballSound() {
        fireballAudio.Play();
    }

    internal void PlayDestroySound() {
        destroyAudio.Play();
    }
}

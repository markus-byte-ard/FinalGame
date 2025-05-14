using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDropHandler {
    public Image playerImage = null;
    public Image mirrorImage = null;
    public Image glowImage = null;
    public Image frozenImage = null;
    
    public Text healthText = null;

    public int maxHealth = 5;
    public int health = 5; // Current health
    public int mana = 1;

    public bool isPlayer;
    public bool isFire;
    public bool isFrozen;
    public int freezeDuration = 1;

    public GameObject[] manaBalls = new GameObject[7];

    private Animator animator = null;

    public AudioSource dealAudio = null;
    public AudioSource healAudio = null;
    public AudioSource mirrorAudio = null;
    public AudioSource smashAudio = null;
    public AudioSource hitAudio = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        animator = GetComponentInChildren<Animator>();
        UpdateHealth();
        UpdateManaBalls();
    }

    internal void PlayHitAnim() {
        if (animator != null) {
            animator.SetTrigger("Hit");
            animator.SetInteger("Health", health);
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if (!GameController.instance.cardIsPlayable) {
            return;
        }
        
        GameObject obj = eventData.pointerDrag;
        if (obj != null) {
            Card card = obj.GetComponent<Card>();
            if (card != null) {
                GameController.instance.UseCard(card, this, GameController.instance.playersHand);
            }
        }
    }

    internal void UpdateHealth() {
        if (health >= 0) {
            healthText.text = health.ToString();
            
        } else {
            Debug.Log("Health is not a valid number" + health.ToString());
            health = 0;
            healthText.text = health.ToString();
        }
    }

    internal void SetMirror(bool on) {
        mirrorImage.gameObject.SetActive(on);
    }

    internal bool HasMirror() {
        return mirrorImage.gameObject.activeInHierarchy;
    }

    internal void UpdateManaBalls() {
        for(int m = 0; m < 7; m++) {
            if (mana > m) {
                manaBalls[m].SetActive(true);
            } else {
                manaBalls[m].SetActive(false);
            }
        }
    }

    internal void Freeze(int duration) {
        isFrozen = true;
        freezeDuration = duration;
        frozenImage.gameObject.SetActive(true);
    }

    internal void Unfreeze() {
        isFrozen = false;
        freezeDuration = 0;
        frozenImage.gameObject.SetActive(false);
    }

    internal void UpdateFreeze() {
        if (isFrozen && freezeDuration > 0) {
            freezeDuration--;
            
            if (freezeDuration <= 0) {
                Unfreeze();
            }
        }
    }

    internal void PlayMirrorSound() {
        mirrorAudio.Play();
    }
    
    internal void PlaySmashSound() {
        smashAudio.Play();
    }

    internal void PlayHealSound() {
        healAudio.Play();
    }

    internal void PlayDealSound() {
        dealAudio.Play();
    }

    internal void PlayHitSound() {
        hitAudio.Play();
    }
}

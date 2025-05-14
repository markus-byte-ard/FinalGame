using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    // Singleton
    static public GameController instance = null;

        // Decks
    public Deck playerDeck = new Deck();
    public Deck enemyDeck = new Deck();

    // Hands
    public Hand playersHand = new Hand();
    public Hand enemysHand = new Hand();

    // Player and enemy
    public Player player = null;
    public Player enemy = null;

    // Cards
    public List<CardData> cards = new List<CardData>();
    
    // UI
    public GameObject cardPrefab = null;
    public Canvas canvas = null;

    // Effect prefabs
    public GameObject effectFromLeftPrefab = null;
    public GameObject effectFromRightPrefab = null;

    // Attack sprites
    public Sprite fireBallImage = null;
    public Sprite iceBallImage = null;
    public Sprite Fire2BallImage = null;
    public Sprite Ice2BallImage = null;
    public Sprite fireAndIceBallImage = null;
    public Sprite destructBallImage = null;
    public Image enemySkipTurn = null;

    // Enemy
    public Sprite fireDemon = null;
        
    // Booleans
    public bool playersTurn = true;
    public bool cardIsPlayable = false;

    // UI Text 
    public Text turnText = null;
    public Text currentTurnText = null;
        
    public int turn = 0;
             
    // Audio
    public AudioSource playerDieAudio = null;
    public AudioSource enemyDieAudio = null;
    public AudioSource musicAudio = null;
    public AudioSource buttonAudio = null;

    private void Awake() {
        // Singleton pattern: ensures only one instance of TextAdventureManager exists
        if (instance == null) {
            instance = this;
            // DontDestroyOnLoad(gameObject); // Keeps this object alive across scenes
        } else {
            Destroy(gameObject); // Destroy this instance if another one exists
        }

        SetUpEnemy();

        SetUpCardBattle();
    }

    private void SetUpCardBattle() {
        // Init turns
        turn = 1;
        UpdateTurn();
        playerDeck.Create();
        enemyDeck.Create();
        StartCoroutine(DealHands());
    }

    public void CallMainMenu() {
        // Loads Main menu scene
        StartCoroutine(MainMenu());
    }
    
    private IEnumerator MainMenu() {
        // Loads Main menu scene
        buttonAudio.Play();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(0);
    }

    public void SkipTurn() {
        if (playersTurn && cardIsPlayable) {
            buttonAudio.Play();
            NextPlayersTurn();
        }
    }

    internal IEnumerator DealHands() {
        yield return new WaitForSeconds(1);
        for (int t = 0; t < 3; t++) {
            // Debug.Log("Dealing card " + (t + 1));
            playerDeck.DealCard(playersHand);
            enemyDeck.DealCard(enemysHand);
            yield return new WaitForSeconds(1);
            // Debug.Log("Wait complete for card " + (t + 1));
        }
        cardIsPlayable = true;
    }

    internal bool UseCard(Card card, Player usingOnPlayer, Hand fromHand) {
        // Check if the player or enemy is dead before using the card
        if (player.health <= 0 || enemy.health <= 0) {
            Debug.Log("Cannot use card, a player is dead!");
            return false;
        }
        // Check if card is not valid
        if (!CardValid(card, usingOnPlayer, fromHand)) {
            return false;
        }

        cardIsPlayable = false;
        
        CastCard(card, usingOnPlayer, fromHand);

        player.glowImage.gameObject.SetActive(false);
        enemy.glowImage.gameObject.SetActive(false);

        fromHand.RemoveCard(card);

        return false;
    }

    internal bool CardValid (Card cardBeingPlayed, Player usingOnPlayer, Hand fromHand) {
        bool valid = false;

        if (cardBeingPlayed == null) {
            return false;
        }
        if (fromHand.isPlayers) {
            if (cardBeingPlayed.cardData.cost <= player.mana) {
                if (usingOnPlayer.isPlayer && cardBeingPlayed.cardData.isDefenseCard) {
                    valid = true;
                }
                if (!usingOnPlayer.isPlayer && !cardBeingPlayed.cardData.isDefenseCard) {
                    valid = true;
                }
            }
        } else {
            if (cardBeingPlayed.cardData.cost <= enemy.mana) {
                if (!usingOnPlayer.isPlayer && cardBeingPlayed.cardData.isDefenseCard) {
                    valid = true;
                }
                if (usingOnPlayer.isPlayer && !cardBeingPlayed.cardData.isDefenseCard) {
                    valid = true;
                }
            }
        }
        return valid;
    }

    internal void CastCard(Card card, Player usingOnPlayer, Hand fromHand) {
        if (card.cardData.isMirrorCard) {
            usingOnPlayer.SetMirror(true);
            usingOnPlayer.PlayMirrorSound();
            NextPlayersTurn();
            cardIsPlayable = true;
        } else {
            if (card.cardData.isDefenseCard) { // Health cards
                usingOnPlayer.health += card.cardData.damage;
                usingOnPlayer.PlayHealSound();
                if (usingOnPlayer.health > usingOnPlayer.maxHealth) {
                    usingOnPlayer.health = usingOnPlayer.maxHealth;
                }
                UpdateHealths();
                StartCoroutine(CastHealEffect(usingOnPlayer));
            } else { // Attack Card
                CastAttackEffect(card, usingOnPlayer);
            }
    
        }
        if (fromHand.isPlayers) {
            GameController.instance.player.mana -= card.cardData.cost;
            GameController.instance.player.UpdateManaBalls();
        } else {
            GameController.instance.enemy.mana -= card.cardData.cost;
            GameController.instance.enemy.UpdateManaBalls();
        }
    }

    private IEnumerator CastHealEffect(Player usingOnPlayer) {
        yield return new WaitForSeconds (0.5f);
        NextPlayersTurn();
        cardIsPlayable = true;
    }

    internal void CastAttackEffect(Card card, Player usingOnPlayer) {
        GameObject effectGO = null;
        if (usingOnPlayer.isPlayer) {
            effectGO = Instantiate(effectFromRightPrefab, canvas.gameObject.transform);
        } else {
            effectGO = Instantiate(effectFromLeftPrefab, canvas.gameObject.transform);
        }

        Effect effect = effectGO.GetComponent<Effect>();

        if (effect) {
            effect.targetPlayer = usingOnPlayer;
            effect.sourceCard = card;

            switch(card.cardData.damageType) {
                case CardData.DamageType.Fire:
                    if (card.cardData.isMulti) {
                        effect.effectImage.sprite = Fire2BallImage;
                    } else {
                        effect.effectImage.sprite = fireBallImage;
                    }
                    effect.PlayFireballSound();
                break;
                case CardData.DamageType.Ice:
                    if (card.cardData.isMulti) {
                        effect.effectImage.sprite = Ice2BallImage;
                    } else {
                        effect.effectImage.sprite = iceBallImage;
                    }
                    effect.PlayIceSound();
                break;
                case CardData.DamageType.Both:
                    effect.effectImage.sprite = fireAndIceBallImage;
                    effect.PlayFireballSound();
                    effect.PlayIceSound();
                break;
                case CardData.DamageType.Destroy:
                    effect.effectImage.sprite = destructBallImage;
                    effect.PlayDestroySound();
                break;
            }
        }
    }

    internal void UpdateHealths() {
        player.UpdateHealth();
        enemy.UpdateHealth();

        if (player.health <= 0) {
            StartCoroutine(GameOver());
        } if (enemy.health <= 0) {
            StartCoroutine(GameEnd());
        }
    }

    private void SetUpEnemy() {
        enemy.mana = 0;
        enemy.UpdateHealth();
        enemy.isFire = true;
        enemy.playerImage.sprite = fireDemon;
    }

    private IEnumerator GameOver() {
        yield return new WaitForSeconds(5);
        PlayerPrefs.SetInt("turns", turn);
        musicAudio.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    private IEnumerator GameEnd() {
        yield return new WaitForSeconds(5);
        musicAudio.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    internal void NextPlayersTurn() {
        if (player.isFrozen) {
            player.UpdateFreeze();  // Update freeze duration for the player
            StartCoroutine(ShowFreezeAndSkipTurn(player));
            if (player.mana < 7) {
                player.mana++;
                player.UpdateManaBalls();
                Debug.Log("Mana1 player frozen");
            }
            playersTurn = !playersTurn;
            turn++;
            SetTurnText();
            UpdateTurn();
            MonstersTurn();
            return;
        }
        
        playersTurn = !playersTurn;
        bool enemyIsDead = false;

        if (playersTurn) {
            player.frozenImage.gameObject.SetActive(false);
            if (player.mana < 7) {
                player.mana++;
                player.UpdateManaBalls();
                Debug.Log("Mana1 player");
            }
        } else {
            if (enemy.isFrozen) {
                enemy.UpdateFreeze();  // Update freeze duration for the enemy
                StartCoroutine(ShowFreezeAndSkipTurn(enemy));
                if (enemy.mana < 7) {
                    enemy.mana++;
                    enemy.UpdateManaBalls();
                    Debug.Log("Mana1 enemy");
                }
                // Kinda OP
                if (player.mana < 7) {
                    player.mana++;
                    player.UpdateManaBalls();
                    Debug.Log("Mana2 player");
                }
                playersTurn = !playersTurn;
                SetTurnText();
                UpdateTurn();
                return;
            }
            enemy.frozenImage.gameObject.SetActive(false);
            if (enemy.health > 0) {
                if (enemy.mana < 7) {
                    enemy.mana++;
                    Debug.Log("Mana2 Enemy");
                }
            } else {
                enemyIsDead = true;
            }
        }

        if (enemyIsDead){
            playersTurn = !playersTurn;
            if (player.mana < 7) {
                player.mana++;
                Debug.Log("Mana3 player");
            }
        } else {
            SetTurnText();
            if (!playersTurn) {
                MonstersTurn();
            } else {
                turn++;
                UpdateTurn();
            }
        }
        player.UpdateManaBalls();
        enemy.UpdateManaBalls();
        UpdateTurn();
    }

    private void MonstersTurn() {
        Card card = AIChooseCard();
        StartCoroutine(MonsterCastCard(card));
    }

    private Card AIChooseCard() {
        List<Card> available = new List<Card>();
        for(int i = 0; i < 3; i++) {
            if (CardValid(enemysHand.cards[i], enemy, enemysHand)) {
                available.Add(enemysHand.cards[i]);
            } else if (CardValid(enemysHand.cards[i], player, enemysHand)) {
                available.Add(enemysHand.cards[i]);
            }
        }
        if (available.Count == 0) {
            NextPlayersTurn();
            return null;
        }
        int choice = UnityEngine.Random.Range(0, available.Count);
        return available[choice];
    }

    private IEnumerator MonsterCastCard(Card card) {
        yield return new WaitForSeconds(0.5f);

        if (card) {
            TurnCard(card);
            yield return new WaitForSeconds(2);
            if (card.cardData.isDefenseCard) {
                UseCard(card, enemy, enemysHand);
            } else {
                UseCard(card, player, enemysHand);
            }

            yield return new WaitForSeconds(1);
            enemyDeck.DealCard(enemysHand);

            yield return new WaitForSeconds(1);
            
        } else {
            enemySkipTurn.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            enemySkipTurn.gameObject.SetActive(false);
        }
    }

    internal void TurnCard(Card card) {
        Animator animator = card.GetComponentInChildren<Animator>();
        if (animator) {
            animator.SetTrigger("Flip");
        } else {
            Debug.LogError("No Animator found");
        }
    }

    internal void SetTurnText() {
        if (playersTurn) {
            turnText.text = "<color=#ffffff>Player's turn</color>";
        } else {
            turnText.text = "<color=#B42525>Enemy's turn</color>";
        }
    }

    private void UpdateTurn() {
        currentTurnText.text = "Turn: " + turn;
    }

    private IEnumerator ShowFreezeAndSkipTurn(Player targetPlayer) {
        // Show the freeze visual feedback (ice image or similar)
        targetPlayer.frozenImage.gameObject.SetActive(true);  // Display freeze image
        yield return new WaitForSeconds(1);  // Wait for 1 second to show the freeze effect
    }
    
    internal void PlayPlayerDieSound() {
        playerDieAudio.Play();
    }
    internal void PlayEnemyDieSound() {
        enemyDieAudio.Play();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adventurePlayer : MonoBehaviour {
    public Location currentLocation;
    public AudioSource ambientMusic = null;
    public AudioSource battleStartMusic = null;
    public AudioSource pickUpItem = null;
    public AudioSource steps = null;
    public AudioSource buttonAudio = null;

    public List<Item> inventory = new List<Item>();

    public bool ChangeLocation(TextAdventureManager controller, string connectionNoun) {
        Connection connection = currentLocation.GetConnection(connectionNoun);

        if (connection != null) {
            if (connection.connectionEnabled) {
                currentLocation = connection.location;
                StartCoroutine(PlayStepSoundMultipleTimes());
                return true;
            }
        }
        return false;
    }

    public IEnumerator Teleport(TextAdventureManager controller, Location destination) {
        currentLocation = destination;
        ambientMusic.Stop();
        battleStartMusic.Play();
        yield return new WaitForSeconds(6);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    IEnumerator PlayStepSoundMultipleTimes() {
        for (int i = 0; i < 3; i++) {
            steps.Play();  // Play one instance of the clip
            yield return new WaitForSeconds(0.4f); // Wait before playing again
        }
    }

    internal bool CanUseItem(TextAdventureManager controller, Item item) {
        if (item.targetItem == null) {
            return true;
        }
        if (HasItem(item.targetItem)) {
            return true;
        }
        if (currentLocation.HasItem(item.targetItem)) {
            return true;
        }
        return false;
    }

    private bool HasItem(Item itemToCheck) {
        foreach(Item item in inventory) {
            if (item == itemToCheck && item.itemEnabled) {
                return true;
            }
        }
        return false;
    }

    internal bool CanTalkToItem(TextAdventureManager controller, Item item) {
        return item.playerCanTalkTo;
    }

    internal bool CanGiveToItem(TextAdventureManager controller, Item item) {
        return item.playerCanGiveTo;
    }

    public bool HasItemByName(string noun) {
        foreach(Item item in inventory) {
            if (item.itemName.ToLower() == noun.ToLower()) {
                return true;
            }
        }
        return false;
    }
}

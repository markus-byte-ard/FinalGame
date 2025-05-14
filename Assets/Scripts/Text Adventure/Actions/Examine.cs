using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Examine")]
public class Examine : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        // Check items in the room
        if (CheckItems(controller, controller.player.currentLocation.items, noun)) {
            return;
        }
        // Check items in the inventory
        if (CheckItems(controller, controller.player.inventory, noun)) {
            return;
        }
        controller.currentText.text = "<color=red>You can't see the "+noun+"</color>\n";
        controller.DisplayLocation(true);
    }

    private bool CheckItems(TextAdventureManager controller, List<Item> items, string noun) {
        foreach(Item item in items) {
            if (item.itemName.ToLower() == noun && item.itemEnabled) {
                if (item.InteractWith(controller, "examine")) {
                    return true;
                }
                controller.currentText.text = "<color=#00ff00ff>You see "+item.description +"</color>\n";
                controller.DisplayLocation(true);
                return true;
            }
        }
        return false;
    }
}

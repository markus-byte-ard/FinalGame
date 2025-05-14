using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Give")]
public class Give : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        // Checks if the inventory has the item
        if (controller.player.HasItemByName(noun)) {
    
            // Check items in the room
            if (GiveToItem(controller, controller.player.currentLocation.items, noun)) {
                return;
            }
            controller.currentText.text = "<color=red>Nothing takes the " +noun + "</color>\n";
            controller.DisplayLocation(true);
            return;
        }
        controller.currentText.text = "<color=red>You don't have the "+noun+" to give</color>\n";
        controller.DisplayLocation(true);
    }

    private bool GiveToItem(TextAdventureManager controller, List<Item> items, string noun) {
        foreach(Item item in items) {
            if (item.itemEnabled) {
                if (controller.player.CanGiveToItem(controller, item)) {
                    if (item.InteractWith(controller, "give", noun)) {
                        return true;
                    }
                }
                controller.currentText.text = "<color=red>No response to "+noun+"</color>\n";
                controller.DisplayLocation(true);
                return true;
            }
        }
        return false;
    }
}

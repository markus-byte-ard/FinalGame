using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Use")]
public class Use : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        // Use items in the room
        if (UseItems(controller, controller.player.currentLocation.items, noun)) {
            return;
        }
        // Use items in the inventory
        if (UseItems(controller, controller.player.inventory, noun)) {
            return;
        }
        controller.currentText.text = "<color=red>There is no "+noun+"</color>\n";
        controller.DisplayLocation(true);
    }

    private bool UseItems(TextAdventureManager controller, List<Item> items, string noun) {
        foreach(Item item in items) {
            if (item.itemName == noun && item.itemEnabled) {
                if (controller.player.CanUseItem(controller, item)) {
                    if (item.InteractWith(controller, "use")) {
                        return true;
                    }
                }
                controller.currentText.text = "<color=red>The "+noun+" does nothing</color>\n";
                controller.DisplayLocation(true);
                return true;
            }
        }
        return false;
    }
}

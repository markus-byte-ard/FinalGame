using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Say")]
public class Say : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        // Check items in the room
        if (SayToItem(controller, controller.player.currentLocation.items, noun)) {
            return;
        }
        controller.currentText.text = "<color=red>Nothing responds</color>\n";
        controller.DisplayLocation(true);
    }

    private bool SayToItem(TextAdventureManager controller, List<Item> items, string noun) {
        foreach(Item item in items) {
            if (item.itemEnabled) {
                if (controller.player.CanTalkToItem(controller, item)) {
                    if (item.InteractWith(controller, "say", noun)) {
                        return true;
                    }
                }
                controller.currentText.text = "<color=red>No response to " + noun + "</color>\n";
                controller.DisplayLocation(true);
                return true;
            }
        }
        return false;
    }
}

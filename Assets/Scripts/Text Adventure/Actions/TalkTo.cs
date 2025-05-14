using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Actions/TalkTo")]
public class TalkTo : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        // Check items in the room and can it be talked to
        if (TalkToItem(controller, controller.player.currentLocation.items, noun)) {
            return;
        }
        controller.currentText.text = "<color=red>There is no " +noun+ " here.</color>\n";
        controller.DisplayLocation(true);
    }

    private bool TalkToItem(TextAdventureManager controller, List<Item> items, string noun) {
        foreach(Item item in items) {
            if (item.itemName == noun && item.itemEnabled) {
                if (controller.player.CanTalkToItem(controller, item)) {
                    if (item.InteractWith(controller, "talkto")) {
                        return true;
                    }
                }
                controller.currentText.text = "<color=red>The "+noun+" doesn't respond</color>\n";
                controller.DisplayLocation(true);
                return true;
            }
        }
        return false;
    }
}

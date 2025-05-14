using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Inventory")]
public class Inventory : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        
        if (controller.player.inventory.Count == 0) {
            controller.currentText.text = "<color=red>You have no items.</color>\n";
            controller.DisplayLocation(true);
            return;
        }
        string result = "<color=#00ff00ff>You have";
        bool first = true;
        foreach (Item item in controller.player.inventory) {
            if (first) {
                result += " the "+item.itemName;
            } else {
                result += " and the "+item.itemName;
            }
            first = false;
        }
        result += "</color>\n";
        controller.currentText.text = result;
        controller.DisplayLocation(true);
    }
}

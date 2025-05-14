using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Take")]
public class Take : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        foreach (Item item in controller.player.currentLocation.items) {
            if (item.itemEnabled && item.itemName == noun) {
                if (item.playerCanTake) {
                    controller.player.inventory.Add(item);
                    controller.player.pickUpItem.Play();
                    controller.player.currentLocation.items.Remove(item);
                    controller.currentText.text = "<color=#00ff00ff>You take the " + noun + "</color>\n";
                    controller.DisplayLocation(true);
                    return;
                }
            }
        }
        controller.currentText.text = "<color=red>You can't take that.</color>\n";
        controller.DisplayLocation(true);
    }
}

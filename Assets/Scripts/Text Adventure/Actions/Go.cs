using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Go")]
public class Go : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        if (controller.player.ChangeLocation(controller, noun)) {
            controller.DisplayLocation();
        } else {
            controller.currentText.text = "<color=red>You can't go that way.</color>\n";
            controller.DisplayLocation(true);
        }
    }
}

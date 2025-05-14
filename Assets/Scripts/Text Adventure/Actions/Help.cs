using UnityEngine;

[CreateAssetMenu(menuName ="Actions/Help")]
public class Help : Action {
    public override void RespondToInput(TextAdventureManager controller, string noun) {
        controller.currentText.text = "<color=#00ff00ff>Type a Verb followed by a noun (e.g. \"go north\")</color>";
        controller.currentText.text += "\n<color=#00ff00ff>Allowed verbs: Go, Examine, Take, Use, Inventory, TalkTo, Say, Give, Help</color>\n";
        controller.DisplayLocation(true);
    }
}

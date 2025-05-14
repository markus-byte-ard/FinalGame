using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextAdventureManager : MonoBehaviour {
    static public TextAdventureManager instance = null;
    
    public adventurePlayer player;

    public InputField textEntryField;
    //public Text logText;
    public TextMeshProUGUI logText;
    //public Text currentText;
    public TextMeshProUGUI currentText;

    public Action[] actions;

    [TextArea]
    public string introText;

    void Awake() {
        // Singleton pattern: ensures only one instance of TextAdventureManager exists
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject); // Keeps this object alive across scenes
        } else {
            Destroy(gameObject); // Destroy this instance if another one exists
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        logText.text = introText;
        DisplayLocation();
        textEntryField.ActivateInputField();
    }

    public void DisplayLocation(bool additive = false) {
        string description = player.currentLocation.description+"\n";
        description += player.currentLocation.GetConnectionsText();
        description += player.currentLocation.GetItemsText();
        if(additive) {
            currentText.text = currentText.text + "\n" + description;
        } else {
            currentText.text = description;
        }
    }

    public void TextEntered() {
        LogCurrentText();
        ProcessInput(textEntryField.text);
        textEntryField.text = "";
        textEntryField.ActivateInputField();
    }

    void LogCurrentText() {
        logText.text += "\n\n";
        logText.text += currentText.text;

        logText.text += "\n\n";
        logText.text += "<color=#aaccaaff>"+textEntryField.text+"</color>";
    }

    void ProcessInput(string input) {
        input = input.ToLower();

        char[] delimiter = {' '};
        string[] separatedWords = input.Split(delimiter);

        foreach(Action action in actions) {
            if (action.keyword.ToLower() == separatedWords[0]) {
                if (separatedWords.Length > 1) {
                    action.RespondToInput(this, separatedWords[1]);
                } else {
                    action.RespondToInput(this, "");
                }
                //DisplayLocation(true);
                return;
            }
        }

        currentText.text = "<color=red>Nothing happens! (having troubel? type Help)</color>\n";
        DisplayLocation(true);
    }

    public void CallQuit() {
        StartCoroutine(Quit());
    }
    
    private IEnumerator Quit() {
        player.buttonAudio.Play();
        yield return new WaitForSeconds(0.25f);
        // Loads Main menu scene
        SceneManager.LoadScene(0);
    }
}

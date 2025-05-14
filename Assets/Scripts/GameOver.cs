using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    public Text turnsText = null;
    public AudioSource buttonSound = null;

    private void Awake() {
        turnsText.text = "Turns to die: "+ PlayerPrefs.GetInt("turns").ToString();
    }

    public void RestartGameCall() {
        StartCoroutine(RestartGame());
    }

    public void MainMenuCall() {
        StartCoroutine(MainMenu());
    }

    private IEnumerator RestartGame() {
        buttonSound.Play();
        yield return new WaitForSeconds(0.25f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    private IEnumerator MainMenu() {
        buttonSound.Play();
        yield return new WaitForSeconds(0.25f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

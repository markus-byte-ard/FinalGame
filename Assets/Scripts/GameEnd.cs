#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameEnd : MonoBehaviour {
    public AudioSource buttonSound = null;

    public void CallCredits() {
        StartCoroutine(Credits());
    }

    public void CallQuit() {
        StartCoroutine(Quit());
    }

    public void CallMainMenu() {
        StartCoroutine(MainMenu());
    }

    private IEnumerator Credits() {
        buttonSound.Play();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(5);
    }

    private IEnumerator MainMenu() {
        buttonSound.Play();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator Quit() {
        buttonSound.Play();
        yield return new WaitForSeconds(0.25f);
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
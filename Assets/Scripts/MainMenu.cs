#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public AudioSource buttonSound = null;

    public void NewGame() {
        StartCoroutine(LoadGame());
    }

    public void QuitGame() {
        StartCoroutine(Quit());
    }

    private IEnumerator LoadGame() {
        buttonSound.Play();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(3);
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
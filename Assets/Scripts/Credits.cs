using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
    public AudioSource buttonSound = null;
    
    public void CallReturn() {
        StartCoroutine(Return());
    }

    private IEnumerator Return() {
        buttonSound.Play();
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(4);
    }
}
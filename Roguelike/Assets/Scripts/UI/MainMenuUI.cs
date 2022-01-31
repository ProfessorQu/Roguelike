using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject main;
    public GameObject tutorial;
    
    private void Start() {
        main.SetActive(true);
        tutorial.SetActive(false);
    }

    public void ShowMain() {
        StartCoroutine(ShowMain(true));
    }

    public void ShowTutorial() {
        StartCoroutine(ShowMain(false));
    }

    private IEnumerator ShowMain(bool show) {
        LevelTransition.Instance.PlayAnimation();

        yield return new WaitForSeconds(1);

        main.SetActive(show);
        tutorial.SetActive(!show);

        LevelTransition.Instance.ResetAnimation();
    }

    public void Quit() {
        Application.Quit();
    }

    public void Play() {
        StartCoroutine(PlayGame());
    }
    
    private IEnumerator PlayGame() {
        LevelTransition.Instance.PlayAnimation();

        yield return new WaitForSeconds(1);
        
        SceneManager.LoadScene(1);
    }
}

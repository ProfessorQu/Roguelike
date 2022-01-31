using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;
    GameObject death;

    public float transitionTime = 1f;

    [Space]
    public int currentLevel = 1;
    public int maxLevel = 5;
    
    [Space]
    public int coins = 0;
    public float time;

    private bool gameOver = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;

            death = GameObject.FindGameObjectWithTag("DeathScreen");
            death.SetActive(false);

            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (!gameOver)
            time += Time.deltaTime;
    }

    public bool LoadNextLevel() {
        if (!gameOver) {
            if (currentLevel < maxLevel && FindObjectOfType<Player>().IsGrounded()) {
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));

                return true;
            }
            else if (currentLevel == maxLevel) {
                Debug.Log("Finished game!");

                return true;
            }
        }

        return false;
    }

    private IEnumerator LoadLevel(int levelIndex) {
        LevelTransition.Instance.PlayAnimation();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
        currentLevel++;
        
        yield return new WaitForSeconds(0.05f);
        
        death = GameObject.FindGameObjectWithTag("DeathScreen");
        death.SetActive(false);
    }

    public void GameOver() {
        gameOver = true;

        death.SetActive(true);
    }

    public void Retry() {
        if (gameOver) {
            StartCoroutine(RetryLevel());
        }
    }

    private IEnumerator RetryLevel() {
        gameOver = false;
        
        LevelTransition.Instance.PlayAnimation();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        currentLevel = 1;
        coins = 0;
        time = 0;
        
        yield return new WaitForSeconds(0.05f);
        
        death = GameObject.FindGameObjectWithTag("DeathScreen");
        death.SetActive(false);
    }
}

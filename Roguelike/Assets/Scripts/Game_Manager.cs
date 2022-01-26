using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    public float transitionTime = 1f;

    [Space]
    public int currentLevel = 0;
    public int maxLevel = 5;
    
    [Space]
    public int coins = 0;
    public float time;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        time += Time.deltaTime;
    }

    public bool LoadNextLevel() {
        if (currentLevel < maxLevel && FindObjectOfType<Player>().IsGrounded()) {
            currentLevel++;
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
            return true;
        }
        else if (currentLevel == maxLevel) {
            Debug.Log("Finished game!");

            return true;
        }

        return false;
    }

    IEnumerator LoadLevel(int levelIndex) {
        LevelTransition.Instance.PlayAnimation();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}

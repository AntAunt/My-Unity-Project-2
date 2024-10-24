using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{
    private bool gameEnded;
    public bool isFinalLevel;
    public int levelNumberSave;

    public GameObject winPanel;
    private GameObject playerObject;
    private PlayerController playerController;

    public void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject.GetComponent<PlayerController>() != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
    }

    public void Update()
    {
        if (playerController != null)
        {
            if (playerController.endLevel)
            {
                WinLevel();
            }
        }
    }


    public void WinLevel()
    {
        if (!gameEnded)
        {
            Debug.Log("You Win! Wahoo");

            if (PlayerPrefs.GetInt("levelDone") < levelNumberSave)
            {
                PlayerPrefs.SetInt("levelDone", levelNumberSave);
            }

            winPanel.SetActive(true);
            gameEnded = true;
            SaveScore();
            Debug.Log("the high score for this level is: " + PlayerPrefs.GetInt(GameManager.levelNames[SceneManager.GetActiveScene().buildIndex]));
        }
    }

    public void LoadNextLevel()
    {
        if (!isFinalLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Debug.Log("Please go to main menu");
        SceneManager.LoadScene("Main Menu");
    }

    public void LoseLevel()
    {
        if (!gameEnded)
        {
            Debug.Log("You Lose! Womp womp.");
            RestartLevel();
            gameEnded = true;  
        }
    }

    private void SaveScore()
    {
        if (GameManager.GetTotalScore() > PlayerPrefs.GetInt(GameManager.levelNames[SceneManager.GetActiveScene().buildIndex]))
        {
            PlayerPrefs.SetInt(GameManager.levelNames[SceneManager.GetActiveScene().buildIndex], GameManager.GetTotalScore());
        }
        Debug.Log("time score: " + GameManager.timeScore);
        Debug.Log("score score: " + GameManager.score);
        GameManager.ResetScore();
    }
}

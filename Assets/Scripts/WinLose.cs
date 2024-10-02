using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{
    private bool gameEnded;
    public string nextLevelName;
    public int levelNumberSave;

    public GameObject winPanel;


    public void WinLevel()
    {
        if (!gameEnded)
        {
            Debug.Log("You Win! Wahoo");

            if(PlayerPrefs.GetInt("levelDone") < levelNumberSave)
            {
                PlayerPrefs.SetInt("levelDone", levelNumberSave);
            }

            winPanel.SetActive(true);
            gameEnded = true;
        }
    }

    public void LoadNextLevel()
    {
        if (nextLevelName != "")
        {
            SceneManager.LoadScene(nextLevelName);
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
}

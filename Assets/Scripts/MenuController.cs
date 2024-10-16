using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject credits;

    private GameObject currentPanel;

    public void Start()
    {
        GameObject currentPanel = mainMenuPanel;
        Debug.Log(currentPanel);
    }

    public void SelectStartGame()
    {
        SceneManager.LoadScene("Level1");

    }

    public void SelectSelectLevel()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);

    }

    public void SelectSpecificLevel(int index)
    {
        SceneManager.LoadScene(index - 1);
    }

    public void SelectCredits()
    {
        mainMenuPanel.SetActive(false);
        credits.SetActive(true);
    }

    public void SelectBack()
    {
        levelSelectPanel.SetActive(false);
        credits.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SelectLeave()
    {
        Application.Quit();
    }
}

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
        Debug.Log("currentPanel set");
    }

    public void SelectStartGame()
    {
        SceneManager.LoadScene("Level1");

    }

    public void SelectSelectLevel()
    {
        currentPanel.SetActive(false);
        currentPanel = levelSelectPanel;
        currentPanel.SetActive(true);

    }

    public void SelectSpecificLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SelectCredits()
    {
        Debug.Log("credit");
        mainMenuPanel.SetActive(false);
        credits.SetActive(true);
    }

    public void SelectBack()
    {
        currentPanel.SetActive(false);
        currentPanel = mainMenuPanel;
        currentPanel.SetActive(true);
    }

    public void SelectLeave()
    {
        Application.Quit();
    }
}

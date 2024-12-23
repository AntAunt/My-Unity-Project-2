using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        SetLevelCheckmarks();
    }

    public void SelectSpecificLevel(int index)
    {
        SceneManager.LoadScene(index);
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

    private void SetLevelCheckmarks()
    {
        GameObject level = new GameObject();

        for (int i = 0; i<10; i++)

        {
            Debug.Log("Checkmark-" + (i + 1));
            level = levelSelectPanel.transform.Find("Level (" + (i + 1) + ")").gameObject;
            if (PlayerPrefs.GetInt(GameManager.levelNames[i + 1]) > 0)
            {
                level.transform.Find("Checkmark-" + (i + 1)).gameObject.SetActive(true);
            }
        }
    }
}

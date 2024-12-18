using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControlScript : MonoBehaviour
{
    public GameObject timeTracker;
    public GameObject player;

    public TMP_Text snowText;
    public TMP_Text timeText;

    private void Update()
    {
        if (snowText && player && player.GetComponent<PlayerController>())
        {
            snowText.SetText("x " + player.GetComponent<PlayerController>().snowballsCollected);
        }
        if (PlayerPrefs.GetInt(GameManager.levelNames[SceneManager.GetActiveScene().buildIndex], GameManager.GetTotalScore()) > 0) {
            if (timeText && timeTracker && timeTracker.GetComponent<Timer>())
            {
                timeText.SetText("Time: " + timeTracker.GetComponent<Timer>().getTime());
            }
        }
        else if (timeText)
        {
            timeText.SetText("");
        }
    }
}

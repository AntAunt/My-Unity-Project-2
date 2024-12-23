using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public event Action EndReachedEvent;
    public WinLose winLoseScript;
    public int par;

    private PlayerController playerController;
    private TMP_Text text;

    void Start()
    {
        if (text = this.GetComponentInChildren<TMP_Text>())
        {
            text.text = par.ToString();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            EndReachedEvent.Invoke();
            Debug.Log("cooler level transition");
            playerController = collision.collider.gameObject.GetComponent<PlayerController>();
            GameManager.SetSizeScore(par, playerController.snowballsCollected);
            playerController.Win(transform.position);
        }
    
    }
}

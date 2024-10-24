using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public event Action EndReachedEvent;
    public WinLose winLoseScript;
    private PlayerController playerController;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            EndReachedEvent.Invoke();
            Debug.Log("cooler level transition");
            playerController = collision.collider.gameObject.GetComponent<PlayerController>();
            playerController.Win(transform.position);
        }
    
    }
}

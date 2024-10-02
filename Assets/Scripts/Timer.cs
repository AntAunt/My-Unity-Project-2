using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int parTime = 0;
    public GoalController goalController;
    
    private float timeTaken = 0.0f;
    private bool timerActive;
    private bool addedScore;

    // Start is called before the first frame update
    void Start()
    {
        timeTaken = 0.0f;
        timerActive = true;
        addedScore = false;
        goalController.EndReachedEvent += StopTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {  
            timeTaken += Time.deltaTime;
            // Debug.Log(timeTaken);
        }
        else if (!addedScore)
        {
            int score = parTime - (int)timeTaken;
            GameManager.AddTimerScore(score);
            Debug.Log("time score: " + score);
            addedScore = true;
        }
    }
    
    public void StopTimer()
    {
        timerActive = false; 
    }
}

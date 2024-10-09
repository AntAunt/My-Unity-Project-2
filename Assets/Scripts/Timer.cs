using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
            int score = parTime - (int)timeTaken;
            GameManager.SetTimerScore(score + 1);
        }
    }
    
    public void StopTimer()
    {
        timerActive = false; 
    }
}

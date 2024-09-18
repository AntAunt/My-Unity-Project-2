using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D controller;

    public float speed = 40.0f;

    float dir = 0.0f;
    bool jump = false;

    void Update ()
    {
        dir = Input.GetAxisRaw("Horizontal") * speed; 

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        controller.Move(dir * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
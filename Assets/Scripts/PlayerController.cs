using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;

    public float speed = 4.0f;
    private float movementDir = 0.0f;
    public float jumpForce = 6.0f;
    private Vector3 sizeVector;
    
    public bool facingRight = true;
    private bool collectedSnow = false;
    private bool shouldJump = false;
    private int snowballsCollected = 0;

    public ContactFilter2D contactFilter;
    private GameManager.Accessory accessory;
    public float timer;

    private bool grounded => body.IsTouching(contactFilter);

    Animator animator;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sizeVector = Vector3.one;
        accessory = GameManager.currentAccessory;
    }

    void Update ()
    {
        movementDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
            shouldJump = true;

        if (collectedSnow)
        {
            OnPickupSnow();
            collectedSnow = false;
        }
        animator.SetBool("Moving", movementDir != 0);
        animator.SetFloat("vSpeed", body.velocity.y);
        animator.SetBool("Jump", shouldJump);
        animator.SetBool("Grounded", grounded);

        timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (shouldJump)
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        body.velocity = new Vector2(movementDir * speed, body.velocity.y);

        if (movementDir > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        else if (movementDir < 0 && facingRight)
        {
            Flip();
        }

        shouldJump = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snow")
        {
            collectedSnow = true;
            GameManager.AddScore(10);
            Destroy(other.gameObject);
        }

        if (other.tag == "Accessory")
        {
            Debug.Log("stylin");
            GameManager.AddScore(20);
            GameManager.ChangeAccessory(GameManager.Accessory.None); // should store which one it is somehow?
            // do something to change the animator
        }
    }

    private void OnPickupSnow()
    {
        Debug.Log("snow gaming");
        if (facingRight)
        {
            sizeVector.x = sizeVector.x + 0.1f;
        }
        else
        {
            sizeVector.x = sizeVector.x - 0.1f;
        }
        sizeVector.y = sizeVector.y + 0.1f;
        sizeVector.z = sizeVector.z + 0.1f;
        jumpForce *= 0.95f;
        speed *= 0.95f;

        snowballsCollected++;

        transform.localScale = sizeVector;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        sizeVector.x = -sizeVector.x;
    }
}
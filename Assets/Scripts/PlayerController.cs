using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;

    public float defaultSpeed = 5.0f;
    public float defaultJumpForce = 9.0f;
    private float speed;
    private float jumpForce;

    private float movementDir = 0.0f;
    private Vector3 defaultSize;
    private Vector3 sizeVector;

    public bool endLevel = false;
    
    public bool facingRight = true;
    public int snowballsCollected = 0;
    private bool collectedSnow = false;
    private bool shouldJump = false;
    private bool nearFan = false;
    private bool won = false;

    public ContactFilter2D contactFilter;
    private GameManager.Accessory accessory;
    private PlayableDirector director;
    public float timer;

    private bool grounded;

    Animator animator;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultSize = transform.localScale;
        sizeVector = defaultSize;
        speed = defaultSpeed;
        jumpForce = defaultJumpForce;
        accessory = GameManager.currentAccessory;
        director = GetComponent<PlayableDirector>();
        grounded = true;
    }

    void Update ()
    {
        if (!won)
        {
            movementDir = Input.GetAxisRaw("Horizontal");
            grounded = IsGrounded();

            if (Input.GetButtonDown("Jump") && grounded)
                shouldJump = true;

            if (collectedSnow)
            {
                snowballsCollected++;
                collectedSnow = false;
            }
            if (Input.GetButtonDown("Fire3") && nearFan)
            {
                snowballsCollected--;
            }
            animator.SetBool("Moving", movementDir != 0);
            animator.SetFloat("vSpeed", body.velocity.y);
            animator.SetBool("Jump", shouldJump);
            animator.SetBool("Grounded", grounded);

            timer += Time.deltaTime;
            CalculateSize(); // maybe only do this if we know there has been a change in snowballsCollected?
            CalculateWeight(); // maybe only do this if we know there has been a change in snowballsCollected?
        }

        if (GameManager.currentAccessory != accessory)
        {
            // change the animation in some way
            Debug.Log("Accessory change!");
            accessory = GameManager.currentAccessory;
        }

        else if (endLevel)
        {

        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit;
        float raycastDistance = 0.5f * sizeVector.y;
        //Raycast to to the floor objects only
        int mask = 1 << LayerMask.NameToLayer("Ground");

        //Raycast downwards
        hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, mask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        if (!won)
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snow")
        {
            collectedSnow = true;
            GameManager.AddScore(10);
            Debug.Log(GameManager.score);
            Destroy(other.gameObject);
        }

        if (other.tag == "Accessory")
        {
            Debug.Log("stylin");
            GameManager.AddScore(20);
            GameManager.ChangeAccessory(GameManager.Accessory.None); // should store which one it is somehow?
            // do something to change the animator
        }
        if (other.tag == "Fan")
        {
            Debug.Log("oh its a fan");
            nearFan = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (nearFan)
        {
            Debug.Log("bye fan");
            nearFan = false;
        }
    }

    private void OnPickupSnow()
    {
        snowballsCollected++;
    }

    private void CalculateSize()
    {
        float sizeScalar = defaultSize.x + (0.1f * snowballsCollected); // 10% increase per snowball
        if (facingRight)
        {
            sizeVector.x = sizeScalar;
        }
        else
        {
            sizeVector.x = -sizeScalar;
        }
        sizeVector.y = sizeScalar;
        sizeVector.z = sizeScalar;
        transform.localScale = sizeVector;
    }

    private void CalculateWeight()
    {
        speed = defaultSpeed * (Mathf.Pow(0.95f, snowballsCollected));
        jumpForce = defaultJumpForce * (Mathf.Pow(0.95f, snowballsCollected));
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

    public void Win(Vector2 goalPos)
    {

        won = true;

        Debug.Log(goalPos);

        Vector2 position = transform.position;
        position.x = goalPos.x - 1;
        position.y = goalPos.y;

        transform.position = position;

        director.Play();
    }
}
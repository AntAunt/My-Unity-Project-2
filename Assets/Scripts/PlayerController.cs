using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    public bool facingRight = true;
    public int snowballsCollected = 0;
    private bool collectedSnow = false;
    private bool shouldJump = false;

    public ContactFilter2D contactFilter;
    private GameManager.Accessory accessory;
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
        grounded = true;
    }

    void Update ()
    {
        movementDir = Input.GetAxisRaw("Horizontal");
        grounded = IsGrounded();

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
    }

    private void OnPickupSnow()
    {
        snowballsCollected++;
        CalculateSize();
        CalculateWeight();
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
}
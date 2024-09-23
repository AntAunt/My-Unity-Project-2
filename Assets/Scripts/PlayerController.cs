using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;

    public float speed = 4.0f;
    private float movementDir = 0.0f;
    public float jumpForce = 6.0f;
    
    public bool facingRight = true;
    private bool collectedSnow = false;
    private bool shouldJump = false;

    public ContactFilter2D contactFilter;

    private bool grounded => body.IsTouching(contactFilter);

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
            Destroy(other.gameObject);
        }
    }

    private void OnPickupSnow()
    {
        Debug.Log("snow gaming");
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
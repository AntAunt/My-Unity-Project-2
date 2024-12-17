using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    public float defaultSpeed = 5.0f;
    public float defaultJumpForce = 9.0f;

    public bool endLevel = false;
    public bool applyAccessory = false;

    public bool facingRight = true;

    public int snowballsCollected = 0;
    public int minFanSize = 0;
    public int fanUseLimit = 999;

    public Animator animator;
    public event Action<Vector3> FanUsedEvent;

    public ContactFilter2D contactFilter;
    public float timer;

    public AudioSource fanFailSfx;
    public AudioSource jumpSfx;

    private Rigidbody2D body;

    private SpriteRenderer spriteRenderer;

    private float speed;
    private float jumpForce;

    private float movementDir = 0.0f;
    private Vector3 defaultSize;
    private Vector3 sizeVector;

    private bool collectedSnow = false;
    private bool shouldJump = false;
    private bool nearFan = false;
    private bool won = false;

    private Vector3 fanLocation = new Vector3(1.0f, 1.0f, 1.0f);

    private GameManager.Accessory accessory;
    private PlayableDirector director;

    private Vector2 victoryJumpArc = new Vector2(0.0f, 0.0f);

    private bool grounded;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            if (Input.GetButtonDown("Fire3"))
            {
                if (nearFan)
                {
                    if (snowballsCollected > minFanSize && fanUseLimit > 0)
                    {
                        snowballsCollected--;
                        fanUseLimit--;
                        FanUsedEvent.Invoke(fanLocation);
                    }
                    else
                    {
                        // play some feedback to let the player know they cant shrink anymore here
                        // maybe they shake a little?
                        fanFailSfx.Play();
                        Debug.Log("fan not strong enough");
                        Debug.Log("minFanSize: " + minFanSize);
                    }
                }
            }
            animator.SetBool("Moving", movementDir != 0);
            animator.SetFloat("vSpeed", body.velocity.y);
            animator.SetBool("Jump", shouldJump);
            animator.SetBool("Grounded", grounded); 
            animator.SetBool("Won", false);
            animator.SetBool("Carrot", false);
            animator.SetBool("Hat", false);
            animator.SetBool("Shades", false);

            timer += Time.deltaTime;
            CalculateSize(); // maybe only do this if we know there has been a change in snowballsCollected?
            CalculateWeight(); // maybe only do this if we know there has been a change in snowballsCollected?

            if (GameManager.currentAccessory != accessory)
            {
                // change the animation in some way
                Debug.Log("Accessory change!");
                accessory = GameManager.currentAccessory;
            }

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
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log(jumpSfx.pitch);
                jumpSfx.pitch = 1.0f - (0.05f * snowballsCollected);
                jumpSfx.Play();
            }

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
            //GameManager.ChangeAccessory(GameManager.Accessory.None); // should store which one it is somehow?
            // do something to change the animator
        }
        if (other.tag == "Fan")
        {
            Debug.Log("oh its a fan");
            if (other.GetComponent<FanController>() != null)
            {
                minFanSize = other.GetComponent<FanController>().minSize;

                if (other.GetComponent<FanController>().limitedUse)
                {
                    fanUseLimit = other.GetComponent<FanController>().useLimit;
                    fanLocation = other.GetComponent<Transform>().position; // this is a key so we know which fan we are at
                }
            }
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
        if (!won) { 
            won = true;
            //animator.applyRootMotion = true;

            Debug.Log(goalPos);

            Vector2 position = transform.position;
            position.x = goalPos.x - 1;
            position.y = goalPos.y;

            transform.position = position;

            animator.SetBool("Won", won);
            animator.SetBool("Carrot", accessory == GameManager.Accessory.Carrot);
            animator.SetBool("Hat", accessory == GameManager.Accessory.Hat);
            animator.SetBool("Shades", accessory == GameManager.Accessory.Sunglasses);

            PlayableAsset playable = director.playableAsset;

            TimelineAsset asset = director.playableAsset as TimelineAsset;
            foreach (var track in asset.GetOutputTracks())
            {
                track.muted = false;
                if (track.name == "Win" && accessory != GameManager.Accessory.None)
                {
                    track.muted = true;
                }
                if (track.name == "Win with Hat" && accessory != GameManager.Accessory.Hat)
                {
                    track.muted = true;
                }
                if (track.name == "Win with Carrot" && accessory != GameManager.Accessory.Carrot)
                {
                    track.muted = true;
                }
                if (track.name == "Win with Shades" && accessory != GameManager.Accessory.Sunglasses)
                {
                    track.muted = true;
                }
            }

            director.RebuildGraph();
            director.Play();


            if (snowballsCollected != 0)
            {
                position.y = position.y + (snowballsCollected * 0.04f);
                transform.position = position;
                Debug.Log("go up!");
                
            }
        }
    }
}
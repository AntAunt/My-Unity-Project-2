using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeightedButton : MonoBehaviour
{
    public int weightRequired = 0;
    public GameObject wall;

    private GameObject playerObject;
    private PlayerController playerController;

    private TMP_Text text;
    public bool pressed = false;
    public Sprite pressedSprite;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject.GetComponent<PlayerController>() != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
        if (text = this.GetComponentInChildren<TMP_Text>()) { 
            text.text = weightRequired.ToString();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (playerController.snowballsCollected >= weightRequired)
            {
                Debug.Log("lotta snow huh");
                GameObject.Destroy(wall);
                GetComponent<SpriteRenderer>().sprite = pressedSprite;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FanController : MonoBehaviour
{
    public int minSize;
    public int useLimit = 999;
    public bool limitedUse = false;
    public Sprite usedSprite;

    private GameObject playerObject;
    private PlayerController playerController;
    private TMP_Text text;

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject.GetComponent<PlayerController>() != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            playerController.FanUsedEvent += OnUsedFan;
        }
        if ((text = this.GetComponentInChildren<TMP_Text>()) && limitedUse)
        {
            text.text = useLimit.ToString();
        }
    }

    public void OnUsedFan(Vector3 assumedLocation)
    {
        // passing the location around like a key so only the fan the player is at acts on the event.
        // ... this is nonsense but it SHOULD be fine.
        if (assumedLocation == GetComponent<Transform>().position)
        {
            useLimit--;
            if (useLimit == 0)
            {
                GetComponent<Animator>().enabled = false;
                GetComponent<SpriteRenderer>().sprite = usedSprite;
            }
            text.text = useLimit.ToString();
        }
    }
}

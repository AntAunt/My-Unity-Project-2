using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryCollectible : MonoBehaviour
{
    public GameManager.Accessory accessory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            GameManager.ChangeAccessory(accessory);
            Destroy(gameObject);
        }
    }
}

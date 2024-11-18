using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryCollectible : MonoBehaviour
{
    public GameManager.Accessory accessory; 
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;


    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
               
        switch (accessory)
        {
            case GameManager.Accessory.Carrot:
                spriteRenderer.sprite = sprites[0];
                break;
            case GameManager.Accessory.Hat:
                spriteRenderer.sprite = sprites[1];
                break;
            case GameManager.Accessory.Sunglasses:
                spriteRenderer.sprite = sprites[2];
                break;
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            GameManager.ChangeAccessory(accessory);
            Destroy(gameObject);
        }
    }
}

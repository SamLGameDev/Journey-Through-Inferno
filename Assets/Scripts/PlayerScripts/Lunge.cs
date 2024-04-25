using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunge : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private Vector2 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartLunge(Rigidbody2D rb, Vector2 direction, SpriteRenderer sprite, Vector2 Pos)
    {
        rb.drag = 100;
        rb.AddForce(direction * 99999 * Time.deltaTime);
        spriteRenderer = sprite;
        playerPos = Pos;
        PlaceAfterImages();
    }
    private void PlaceAfterImages()
    {
        GameObject afterImage = new GameObject();
        SpriteRenderer objectSprite = afterImage.AddComponent<SpriteRenderer>();
        objectSprite.sprite = spriteRenderer.sprite;
        objectSprite.material.color = new Color(0, 0, 0, 1);
        afterImage.transform.localScale = new Vector2(1.5f, 1.5f);
        afterImage.transform.position = playerPos;
        Destroy(afterImage, 0.1f);
        

    }
}

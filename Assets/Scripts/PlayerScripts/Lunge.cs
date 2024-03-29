using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunge : MonoBehaviour
{
    private bool isLunging = false;
    SpriteRenderer spriteRenderer;
    private GameObject afterImage;
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
        isLunging = true;
    }
    public IEnumerator PlaceAfterImages()
    {
        while (true)
        {
            yield return new WaitUntil(() => isLunging);
            afterImage = new GameObject();
            SpriteRenderer objectSprite = afterImage.AddComponent<SpriteRenderer>();
            afterImage.AddComponent<Transform>();
            objectSprite.sprite = spriteRenderer.sprite;
            objectSprite.material.color = new Color(0, 0, 0, 1);
            afterImage.transform.localScale = new Vector2(1.5f, 1.5f);
            afterImage.transform.position = playerPos;
            Destroy(afterImage, 0.1f);
            isLunging = false;
        }

    }
}

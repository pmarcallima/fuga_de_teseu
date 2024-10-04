using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    public Sprite frontSprite;    
    public Sprite backSprite;     
    public Sprite leftSprite;     
    public Sprite rightSprite; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    
    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        moveDirection = moveDirection.normalized;

           if (moveDirection.x != 0) 
        {
            spriteRenderer.flipX = moveDirection.x < 0; 
        }
        
        
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}

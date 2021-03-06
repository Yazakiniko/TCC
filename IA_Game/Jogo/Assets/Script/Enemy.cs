using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    public float speed = 3f;
    public Rigidbody2D rb;
    public LayerMask groundLayers;
    public Transform groundCheck;
    bool isFacingRight = true;
    RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        hit = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayers);
    }

    private void FixedUpdate()
    {
        if(hit.collider != false){
            if(isFacingRight){
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else{
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
        }
        else{
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1f);
        }
    }

    
}

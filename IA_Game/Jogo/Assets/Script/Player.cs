﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;



public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider2d;
    public Transform respawnPoint;
    [SerializeField] GameObject PlayerZ;
    
    public static float deathPosition=1;
    public float speed;
    public float jumpforce;
    public bool isJumping;
    public static int score=0, hp=3,consecutiveFallDeath=0,consecutiveVictory=0;
    public bool doubleJump = false,hasDoubleJump=false;
    private Rigidbody2D rig;
    private Animator anim;

    
    // Start is called before the first frame update
    void Start()
    {
        isJumping = false;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if(hasDoubleJump){
        doubleJump = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        move();
        jump();
        anima();
        detectDeath();

    }

    // função que detecta a movimentação do player
    void move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * speed;

        if(Input.GetAxis("Horizontal") > 0f)
        {
            anim.SetBool("Run", true);
            transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        if(Input.GetAxis("Horizontal") < 0f)
        {
            anim.SetBool("Run", true);
            transform.eulerAngles = new Vector3(0f,180f,0f);
        }
        if(Input.GetAxis("Horizontal") == 0f)
        {
            anim.SetBool("Run", false);
        }
    }

    // função que cria a possibilidade de pular
    void jump()
    {   
        if(hasDoubleJump)
        {            
            if(Input.GetButtonDown("Jump"))
            {
                if(!isJumping)
                {
                    rig.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
                    doubleJump = true;
                }
                else
                {
                    if(doubleJump)
                    {
                        rig.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
                        doubleJump = false;
                    }
                }
            }
        }
        if(!hasDoubleJump)
        {
            if(Input.GetButtonDown("Jump"))
            {
                if(!isJumping)
                {
                    rig.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
                    doubleJump = true;
                }
            }
        }
        
    }
    void detectDeath()
    {
        if(transform.position.y<=-20)
        {
            isJumping = false;
            score-=5;
            deathPosition=this.transform.position.x;
            consecutiveFallDeath++;
            if(score<0){
                score = 0;
            }        
            transform.position = new Vector2(-5,10); 
            ProceduralGenerationScript.destroy = true;
            hp=3;
            consecutiveVictory = 0;
            if(consecutiveFallDeath>=3)
                hasDoubleJump=true;
        }
       
    }

    //Função auto executavel do unity para detectar colisao
    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {   

            isJumping = false;

        }
        if(collision.gameObject.tag == "Finish")
        {

            score+=15;
            isJumping = false;

            if(hp==3){
                ProceduralGenerationScript.comprimento+=20;
                if(ProceduralGenerationScript.distancia<7)
                    ProceduralGenerationScript.distancia++;
                Enemy.speed+=1;
                
            }

            transform.position = new Vector2(-5,10);
            ProceduralGenerationScript.destroy = true;
            consecutiveFallDeath=0;
            consecutiveVictory++;
            if(consecutiveVictory>=3)
                hasDoubleJump=false;
            hp = 3;

        }
        if(collision.gameObject.tag == "Enemy")
        {
            if(hp>1){
                hp--;
                rig.AddForce(new Vector2(0, 1.5f), ForceMode2D.Impulse);
               

            }else
            {

            isJumping = false;
            score-=5;

            if (score<0){
                score=0;
            }
            consecutiveVictory = 0; 
            transform.position = new Vector2(-5,10);
            ProceduralGenerationScript.destroy = true; 
            hp=3;  
            }
        }

    }


    // Função auto executavel do unity para detectar pulo e falta de colisao
    void OnCollisionExit2D (Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            isJumping = true;
        }
    }

    // Usada para chegar o estado para definir a animação a ser executada
    void anima()
    {
        if(Input.GetAxis("Vertical") >= 0f)
        {
            anim.SetBool("Jump", true);
            anim.SetBool("Fall", false);
        }
        if(rig.velocity.y < -0.1)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
        }
        
        if(isJumping == false)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
        }
    }

}  

    


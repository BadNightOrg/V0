﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{
    //Variables
    //Movement
    float translation_H;
    float translation_V;
    Vector2 updated_PlayerVelocity;

    //Mode
    bool inAir;
    bool canJump;
    bool moving_Forced_H;
    float direction_H;
    bool wearMask;
    //int whichMask

    //Properties
    Rigidbody2D playerRB;
    Transform playerTF;
    BoxCollider2D playerCL;
    Animator playerAN;
    
    //Properties to be Adjusted
    [SerializeField] float velocity_H = 1f;
    [SerializeField] float inAir_Velocity_H = 1.5f;
    [SerializeField] float inAir_Velocity_V = 5f;
    // [SerializeField] bool canJump;


    //Interaction Needed GameObject
    [SerializeField] Rigidbody2D Attack;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerTF = GetComponent<Transform>();
        playerCL = GetComponent<BoxCollider2D>();
        playerAN = GetComponent<Animator>();

        canJump = true;
        wearMask = false;
    }

    void Update()
    {
        //Interaction Detect
        translation_H =  Input.GetAxis("Horizontal");
        translation_V = Input.GetAxis("Vertical");

        //Mode Detect
        velocity_Control();
        detectCanJump();
        animation_Control();


        //Interaction Control(can be a function later)
        if(Mathf.Abs(translation_H)>0){
            
            if(Mathf.Sign(playerRB.velocity.x) != translation_H ){
                flipSprite();
            }

            move();
            moving_Forced_H = true;
        }else{
            moving_Forced_H = false;
            
        }

        if(translation_V>0){
            Jump();
            
        }

        if(Input.GetButtonDown("Fire1")){
            attack();
        }


    }

    //This only return whether the move is caused by user interaction
    void move(){
        //Need to be replaced with crossplatform Input manager
        updated_PlayerVelocity = new Vector2(translation_H*velocity_H,playerRB.velocity.y);
        playerRB.velocity = updated_PlayerVelocity;
 
    }

    void Jump()
    {
        if(canJump)
        {
            updated_PlayerVelocity = new Vector2(playerRB.velocity.x, translation_V*inAir_Velocity_V);
            playerRB.velocity = updated_PlayerVelocity;
        }
    }

     private void flipSprite()
    {
        direction_H = (Mathf.Sign(playerRB.velocity.x));
        playerTF.localScale = new Vector2(direction_H, 1f);
        //playerAN.SetBool("Running", goingRightorLeft);
    }

    private void attack(){
        Rigidbody2D clone;
        Vector3 attackPosition = new Vector3((transform.position.x+2*Mathf.Sign(playerRB.velocity.x)),transform.position.y,transform.position.z);
        clone = Instantiate(Attack, attackPosition, transform.rotation);
    }


    //State Controll functiion
    void detectCanJump(){
        if(!playerCL.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            canJump = false;
        }else{
            canJump = true;
        }
    }


    void velocity_Control(){
        //TODO:Inair detection? what else situation besides not touching ground
        if(inAir){
            velocity_H = inAir_Velocity_H;
        }
    }

    void animation_Control(){

        //TODO: inair and Jump condition should also be considered
        if(moving_Forced_H){
            playerAN.SetBool("Walking", true);
        }else{
            playerAN.SetBool("Walking",false);
        }
        
    }


    //Interaction with Other Elements
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Mask"){
            //Before Destroying, read the information
            Destroy(other.gameObject);
            wearMask = true;
            playerAN.SetBool("Masked",true);
        }
    }

    
}

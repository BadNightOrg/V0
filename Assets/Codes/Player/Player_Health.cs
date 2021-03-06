﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Health : MonoBehaviour
{
    int health;
    int numberOfHearts;

    [SerializeField] GameObject[] hearts;

    void Start()
    {
        health = 50;
    }

    // Update is called once per frame
    void Update()
    {
        numberOfHearts = health/10;
        for(int i = 0; i < hearts.Length;i++){
            if(i < numberOfHearts){
                hearts[i].SetActive(true);
            }else{
                hearts[i].SetActive(false);
            }
        }

        life_Controll();
    }

    void life_Controll(){
        if(health <= 0){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy"){
            health -= 10;
        }
    }
}

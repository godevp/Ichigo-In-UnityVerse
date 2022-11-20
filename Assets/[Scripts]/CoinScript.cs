using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 
 Source file Name - CoinScript.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 10/20/2022 
 Program description: coin behaviour script

 */
public class CoinScript : MonoBehaviour
{
    [Range(5, 100)]
    public int coinScore;
    private PlayerController playerController;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            playerController._score += coinScore;
        }
    }
}

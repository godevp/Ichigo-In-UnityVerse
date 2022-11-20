using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 
 Source file Name - Portal.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: on trigger with the object to which the script is attached the player will be teleported to another position
 */
public class Portal : MonoBehaviour
{
    private PlayerController player;
    public Transform whereToTeleport;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = whereToTeleport.position;
        }
    }
}

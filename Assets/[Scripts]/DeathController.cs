using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/*
 
 Source file Name - DeathController.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 10/20/2022 
 Program description: continue for our respawn for player

 */

public class DeathController : MonoBehaviour
{
    public Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Respawn(collision.gameObject);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }

    void Respawn(GameObject go)
    {
        go.transform.position = playerSpawnPoint.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 
 Source file Name - CheckPoint.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 10/20/2022 
 Program description: set a checkpoint for our player when we walk trough it

 */
public class CheckPoint : MonoBehaviour
{
    public Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<DeathController>().playerSpawnPoint = transform;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Respawn(collision.gameObject);
        }
    }

    void Respawn(GameObject go)
    {
        go.transform.position = playerSpawnPoint.position;
    }
}

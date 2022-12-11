using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/*
 
 Source file Name - DeathController.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 12/11/2022
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
            if(collision.gameObject.GetComponent<EnemyController>().isActive)
            {
                collision.gameObject.SetActive(false);
                StartCoroutine(ActivateAfterSomeTime(collision.gameObject, collision.gameObject.GetComponent<EnemyController>().coinPrefab, collision.gameObject.GetComponent<EnemyController>().coinsParent.transform));
            }
        }
    }
    public IEnumerator ActivateAfterSomeTime(GameObject go,GameObject prefab, Transform parent)
    {
        go.GetComponent<EnemyController>().isActive = false;
        var coin = Instantiate(prefab, parent);
        coin.transform.position = go.transform.position;
        yield return new WaitForSeconds(EnemyManager.instance.respawnTime);
        go.SetActive(true);
        go.GetComponent<EnemyController>().isActive = true;
        go.GetComponent<EnemyController>().health = go.GetComponent<EnemyController>().maxHealth;
        go.transform.position = go.GetComponent<EnemyController>().startPos;

    }
    void Respawn(GameObject go)
    {
        go.transform.position = playerSpawnPoint.position;
    }
}

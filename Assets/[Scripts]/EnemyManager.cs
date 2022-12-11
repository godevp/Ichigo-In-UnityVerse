using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<EnemyController> enemyList;
    public static EnemyManager instance;
    public float respawnTime;
    private void Start()
    {
       enemyList = new List<EnemyController>();
       
       foreach(var x in FindObjectsOfType<EnemyController>())
       {
            enemyList.Add(x);
        }
        instance = this;
    }

    private void Update()
    {
       for(int i = 0; i< enemyList.Count; i++)
        {
            if (enemyList[i].health <= 0)
            {
                if (enemyList[i].isActive)
                {
                    enemyList[i].gameObject.SetActive(false);
                    StartCoroutine(ActivateAfterSomeTime(enemyList[i].gameObject, i));
                }
            }
        }
    }
    public IEnumerator ActivateAfterSomeTime(GameObject go, int index)
    {
        go.GetComponent<EnemyController>().isActive = false;
        var coin = Instantiate(enemyList[index].coinPrefab, enemyList[index].coinsParent.transform);
        coin.transform.position = go.transform.position;
        coin.GetComponent<CoinScript>().coinScore = go.GetComponent<EnemyController>().coinPoints;
        yield return new WaitForSeconds(respawnTime);
        go.SetActive(true);
        go.GetComponent<EnemyController>().isActive = true;
        go.GetComponent<EnemyController>().health = go.GetComponent<EnemyController>().maxHealth;
        go.transform.position = go.GetComponent<EnemyController>().startPos;

    }

}

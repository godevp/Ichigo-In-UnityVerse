using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private LinkedList<EnemyController> enemyList;

    private void Start()
    {
       enemyList = new LinkedList<EnemyController>();
       var temp = FindObjectsOfType<EnemyController>();
       foreach(EnemyController x in temp)
        {
            enemyList.AddLast(x);
            Debug.Log(enemyList.Count);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 
 Source file Name - GetsugaManager.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: manager for getsuga where we hold our queue for next POOL logic.

 */
public class GetsugaManager : MonoBehaviour
{
    public GameObject getsugaPrefab;
    private PlayerController playerController;
    private Queue<GameObject> GetsugaPool;
    [Header("Getsuga Properties")]
    [Range(10, 30)]
    public int getsugaNumber = 25;
    public int getsugaCount = 0;
    public int activeGetsuga = 0;

    public float horizontalDistance = 25.0f;
    public float verticalDistance = 25.0f;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        GetsugaPool = new Queue<GameObject>(); // creates an empty queue container
        BuildGetsugaPools();
    }
    void BuildGetsugaPools()
    {
        for (int i = 0; i < getsugaNumber; i++)
        {
            GetsugaPool.Enqueue(CreateGetsuga());
        }

        getsugaCount = GetsugaPool.Count;
    }

    public GameObject GetBullet(Vector2 position)
    {
        GameObject getusga = null;

       
         if (GetsugaPool.Count < 1)
         {
            GetsugaPool.Enqueue(CreateGetsuga());
         }
        getusga = GetsugaPool.Dequeue();
        // stats
        getsugaCount = GetsugaPool.Count;
        activeGetsuga++;


        getusga.SetActive(true);
        getusga.transform.position = position;
        getusga.transform.localScale = new Vector3(playerController.transform.localScale.x, transform.localScale.y, transform.localScale.z);
        getusga.GetComponent<Getsuga>().horizontalBounds.min = getusga.transform.position.x - horizontalDistance;
        getusga.GetComponent<Getsuga>().horizontalBounds.max = getusga.transform.position.x + horizontalDistance;
        getusga.GetComponent<Getsuga>().verticalBounds.min = getusga.transform.position.y - verticalDistance;
        getusga.GetComponent<Getsuga>().verticalBounds.max = getusga.transform.position.y + verticalDistance;


        return getusga;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);


        GetsugaPool.Enqueue(bullet);
        //stats
        getsugaCount = GetsugaPool.Count;
        activeGetsuga--;
    }

    public GameObject CreateGetsuga()
    {
        GameObject _getsuga = Instantiate(getsugaPrefab);
        _getsuga.name = "Getsuga";
        _getsuga.transform.SetParent(transform);
        _getsuga.SetActive(false);
        return _getsuga;
    }
}

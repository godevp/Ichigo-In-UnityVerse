using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public LayerMask collisionLayerMask;
    public bool playerDetected;
    public bool canJumpToPlayer;
    public bool LOS;
    private Transform LOSTransform;
    private LayerMask playerMask;
    public Collider2D collider;



    float radius;
    // Start is called before the first frame update
    void Start()
    {
        playerDetected = false;
        LOS = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDetected)
        {
            canJumpToPlayer = Physics2D.OverlapCircle(LOSTransform.position, radius, playerMask);
            var hit = Physics2D.Linecast(transform.position, LOSTransform.position, collisionLayerMask);
            collider = hit.collider;
            LOS = (hit.collider.gameObject.tag == "Player");
            Debug.Log(LOS);
        }
        //Debug.Log("Can jump to Player : " + canJumpToPlayer);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerDetected = true;
            LOSTransform = collision.gameObject.GetComponent<PlayerController>().pointForEnemyToLookAt;
            radius = collision.gameObject.GetComponent<PlayerController>().radiusForEnemy;
        }
    }

    public void setMaskFromParent(LayerMask mask)
    {
        playerMask = mask;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (LOS) ? Color.green : Color.red;
        if(playerDetected)
        {
            Gizmos.DrawLine(transform.position, LOSTransform.transform.position);
        }
        Gizmos.color = (playerDetected) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 7.0f);
    }
}

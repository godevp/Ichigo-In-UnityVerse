using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 
 Source file Name - PlayerDetection.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 12/11/2022 
 Program description: big part of player detection for enemy;

 */
public class PlayerDetection : MonoBehaviour
{
    public LayerMask collisionLayerMask;
    public bool playerDetected;
    public bool canJumpToPlayer;
    public bool LOS;
    public Transform LOSTransform;
    private LayerMask playerMask;



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
        if (playerDetected)
        {

            var hit = Physics2D.Linecast(transform.position, LOSTransform.position, collisionLayerMask);
            LOS = (hit.collider.gameObject.tag == "Player");
            if (LOS)
            {
                canJumpToPlayer = Physics2D.OverlapCircle(LOSTransform.position, radius, playerMask);
            }
            else
            {
                canJumpToPlayer = false;
            }

        }

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

    public bool CanSeeThePlayer()
    {
        bool canFollow = false;
        if(LOSTransform != null)
        {
            float distanceToPlayer = (transform.position - LOSTransform.transform.position).magnitude;
            canFollow = (LOS && playerDetected && canJumpToPlayer && distanceToPlayer < 9.0f);
        }
        return canFollow;
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

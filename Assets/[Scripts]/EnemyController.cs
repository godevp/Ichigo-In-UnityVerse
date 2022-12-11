using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float horizontalSpeed = 10.0f;

    public PlayerDetection playerDetection;


    public Transform inFrontPoint;
    public Transform AheadPoint;
    public Transform groundPoint; // the origin of the circle
    public Transform otherEnemiesPoint;
    public float groundRadius; // the size of the circle
    public LayerMask groundLayerMask; // the stuff we can collide with
    public LayerMask maskForAnotherEnemies;
    public LayerMask playerMask;


    public bool isObstacleAhead;
    public bool isAnotherEnemyAhead;
    public bool isPlayerAhead;
    public bool isGroundAhead;
    public bool isGrounded = true;

    public Vector2 direction;


    private void Start()
    {
        direction = Vector2.left;
        playerDetection.setMaskFromParent(playerMask);
    }

    private void Update()
    {
        isObstacleAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, groundLayerMask);
        isAnotherEnemyAhead = Physics2D.Linecast(otherEnemiesPoint.position, inFrontPoint.position, maskForAnotherEnemies);
        isPlayerAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, playerMask);
        isGroundAhead = Physics2D.Linecast(groundPoint.position, AheadPoint.position, groundLayerMask);

        
        var hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;
        
        if (isGroundAhead && isGrounded)
        {
            Movement();
        }
        if(!isGrounded || isObstacleAhead || !isGroundAhead || isAnotherEnemyAhead)
        {
            Flip();
        }

       
    }

    public void Movement()
    {
        transform.position += new Vector3(direction.x * horizontalSpeed * Time.deltaTime, 0.0f);
    }
    public void Flip()
    {
        var x = transform.localScale.x * -1.0f;
        direction *= -1.0f;
        transform.localScale = new Vector3(x,transform.localScale.y, transform.localScale.z);
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);

        Gizmos.DrawLine(groundPoint.position, inFrontPoint.position);
        Gizmos.DrawLine(groundPoint.position, AheadPoint.position);
    }
}

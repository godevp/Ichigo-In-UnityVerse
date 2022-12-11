using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float horizontalSpeed = 10.0f;

    public PlayerDetection playerDetection;

    [Range(0,1000)]
    public float health;
    float maxHealth;

    public Transform inFrontPoint;
    public Transform AheadPoint;
    public Transform groundPoint; // the origin of the circle
    public Transform otherEnemiesPoint;
    public float groundRadius; // the size of the circle
    public LayerMask groundLayerMask; // the stuff we can collide with
    public LayerMask maskForAnotherEnemies;
    public LayerMask playerMask;
    public GameObject coinPrefab;
    public GameObject coinsParent;

    public float delayBeforeDeath;

    private Rigidbody2D _rb;

    public bool isObstacleAhead;
    public bool isAnotherEnemyAhead;
    public bool isPlayerAhead;
    public bool isGroundAhead;
    public bool isGrounded = true;

    public GameObject healthCanvas;
    public UnityEngine.UI.Slider healthBar;

    public Vector2 direction;


    private void Start()
    {
        direction = Vector2.left;
        playerDetection.setMaskFromParent(playerMask);
        maxHealth= health;
        healthBar = GetComponentInChildren<UnityEngine.UI.Slider>();
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
        _rb= GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        isObstacleAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, groundLayerMask);
        isAnotherEnemyAhead = Physics2D.Linecast(otherEnemiesPoint.position, inFrontPoint.position, maskForAnotherEnemies);
        isPlayerAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, playerMask);
        isGroundAhead = Physics2D.Linecast(groundPoint.position, AheadPoint.position, groundLayerMask);

        
        var hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;


        if (isGroundAhead && isGrounded && !playerDetection.CanSeeThePlayer())
        {
            Movement();
        }
        if(!isGrounded || isObstacleAhead || !isGroundAhead || isAnotherEnemyAhead)
        {
            Flip();
        }

        if(playerDetection.CanSeeThePlayer())
        {
           // Debug.Log("JUMP");
           // TO DO: ENEMY Should jump to player
        }
       healthBar.value = health;


        if(health <= 0)
        {
            health = 0;
            StartCoroutine(DelayBeforeDeath(delayBeforeDeath));
        }
    }

    private IEnumerator DelayBeforeDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        var coin = Instantiate(coinPrefab, coinsParent.transform);
        coin.transform.position = transform.position;
    }
    public void Movement()
    {
        transform.position += new Vector3(direction.x * horizontalSpeed * Time.deltaTime, 0.0f);
        _rb.AddForce(new Vector3(direction.x * horizontalSpeed,0));
    }
    public void Flip()
    {
        var x = transform.localScale.x * -1.0f;
        var _x = healthCanvas.transform.localScale.x * -1.0f;
        direction *= -1.0f;
        healthCanvas.transform.localScale = new Vector3(_x, healthCanvas.transform.localScale.y, healthCanvas.transform.localScale.z);
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

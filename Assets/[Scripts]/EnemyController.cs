using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float horizontalSpeed = 10.0f;
    private Rigidbody2D _rb;
    public bool isObstacleAhead;
    public bool isAnotherEnemyAhead;
    public bool isPlayerAhead;
    public bool isGroundAhead;
    public bool isGrounded = true;
    public Transform inFrontPoint;
    public Transform AheadPoint;
    public Transform groundPoint; // the origin of the circle
    Vector3 dir;
    public GameObject healthCanvas;
    public UnityEngine.UI.Slider healthBar;
    public Transform otherEnemiesPoint;
    public float groundRadius; // the size of the circle
    public LayerMask groundLayerMask; // the stuff we can collide with
    public LayerMask maskForAnotherEnemies;
    public Vector2 direction;
    public PlayerDetection playerDetection;
    public Vector3 startPos;
    
    [Header("Stats")]
    [Range(0,1000)]
    public float maxHealth;
    public float health;


    [Header("Coins")]
    public GameObject coinPrefab;
    public GameObject coinsParent;

   

    public float delayBeforeDeath;
    [Header("Attack")]
    public float jumpDistance;
    public float simpleAttackDistance;
    public GameObject chaseTarget;
    public LayerMask playerMask;
    public Transform whereToCheck;
    public bool shouldDash;
    public bool startedSimpleAttack;
    [Header("Anim")]
    public Animator _animator;
    public float attackForce;

    public bool isActive = true;

    [Header("Anim")]
    public AudioSource aSource;
    private void Start()
    {
        direction = Vector2.left;
        playerDetection.setMaskFromParent(playerMask);
        health = maxHealth;
        healthBar = GetComponentInChildren<UnityEngine.UI.Slider>();
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
        _rb= GetComponent<Rigidbody2D>();

        if(coinsParent ==null)
        coinsParent = GameObject.Find("[COINS]");
        _animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    private void Update()
    {
        isObstacleAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, groundLayerMask);
        isAnotherEnemyAhead = Physics2D.Linecast(otherEnemiesPoint.position, inFrontPoint.position, maskForAnotherEnemies);
        isPlayerAhead = Physics2D.Linecast(groundPoint.position, inFrontPoint.position, playerMask);
        isGroundAhead = Physics2D.Linecast(groundPoint.position, AheadPoint.position, groundLayerMask);

        shouldDash = Physics2D.Linecast(inFrontPoint.position, whereToCheck.position, playerMask);
        var hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;


        

        if (isGroundAhead && isGrounded &&!playerDetection.CanSeeThePlayer())
        {
            Movement();
        }
        if(!isGrounded || isObstacleAhead || !isGroundAhead || isAnotherEnemyAhead)
        {
            Flip();
        }

        if (playerDetection.CanSeeThePlayer())
        {
            chaseTarget = playerDetection.LOSTransform.gameObject;
            dir = transform.position - chaseTarget.transform.position;
            if (dir.sqrMagnitude < simpleAttackDistance)
            {
                SimpleAttack();
            }
            if(dir.sqrMagnitude > simpleAttackDistance)
            {
                _animator.ResetTrigger("Attack");
            }
            if(dir.sqrMagnitude > (simpleAttackDistance + 0.1f) && dir.sqrMagnitude < jumpDistance && shouldDash)
            {
                DashAttack();
            }
        }
        if (!playerDetection.CanSeeThePlayer())
        {
            chaseTarget = null;
            _animator.ResetTrigger("DashAttack");
        }
            

        healthBar.value = health;


       /* if(health <= 0)
        {
            health = 0;
            StartCoroutine(DelayBeforeDeath(delayBeforeDeath));
        }*/
    }
    void SimpleAttack()
    {
        aSource.clip = AudioManager.instance.ReturnClipWithName("monsterHit");
        if (!aSource.isPlaying)
            aSource.Play();
        _animator.SetTrigger("Attack");
        if(!startedSimpleAttack)
        StartCoroutine(simpleEnemyAttack(0.23f));
        
    }
    private IEnumerator simpleEnemyAttack(float delay)
    {
        startedSimpleAttack = true;
            var attackSphere = Physics2D.OverlapCircle(playerDetection.transform.position, 0.3f, playerMask);
            if (attackSphere && attackSphere.gameObject.tag == "Player")
            {
                attackSphere.gameObject.GetComponent<PlayerController>()._health -= 2.0f;
            }
        yield return new WaitForSeconds(delay);
        startedSimpleAttack = false;
    }
    void DashAttack()
    {
        //AudioManager.instance.PlayThisClipEnemy("monsterHit");
        aSource.clip = AudioManager.instance.ReturnClipWithName("monsterHit");
        if(!aSource.isPlaying)
        aSource.Play();
        _animator.SetTrigger("DashAttack");
        _rb.AddForce(new Vector3(direction.x * attackForce, (Vector2.up * 1).x),ForceMode2D.Impulse);
        
    }


    private IEnumerator DelayBeforeDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        
    }
    public void Movement()
    {
       _rb.AddForce(new Vector3(direction.x * horizontalSpeed,0),ForceMode2D.Force);
        var clampedXVelocity = Mathf.Clamp(_rb.velocity.x,-horizontalSpeed, horizontalSpeed);
        _rb.velocity = new Vector2(clampedXVelocity, _rb.velocity.y);
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


        Gizmos.color = Color.black;
        Gizmos.DrawLine(inFrontPoint.position, whereToCheck.position);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(playerDetection.transform.position, 0.3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>()._health -= 10.0f;
        }
    }

}

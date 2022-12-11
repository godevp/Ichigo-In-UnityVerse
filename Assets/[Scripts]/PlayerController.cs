using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public enum PlayerAnimationState
{
    IDLE,           //0
    WALK,           //1
    JUMP,           //2
    ATTACK,         //3
    ATTACKED        //4
}
/*
 
 Source file Name - PlayerController.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 11/20/2022 
 Program description: main player controller
 */

public class PlayerController : MonoBehaviour
{
    //private
    private Rigidbody2D _rb;
    private PlayerAnimationState animationState;
    private Animator animator;
    //public
    [Header("Movement Properties")]
    public float horizontalSpeed = 10.0f;
    public float horizontalForce = 10.0f;
    public float verticalForce;
    public float airFactor = 5.0f;
    public bool isGrounded = true;
    public Transform groundPoint; // the origin of the circle
    public Transform pointForEnemyToLookAt;
    public float radiusForEnemy;
    public float groundRadius; // the size of the circle
   
    public LayerMask groundLayerMask; // the stuff we can collide with


    [Header("ForAttacking")]
    public LayerMask enemyLayerMask;
    public Transform AttackCenterArea;
    public float AttackRadius;
    public bool attacked = false;
    public bool usedGetsuga = false;

    [Header("Controls")]
    public Joystick leftStick;
    [Range(0.1f, 1.0f)]
    public float verticalThreshold;
    public PhysicsMaterial2D physMat;

    [Header("Sharebale")]
    public int _score;
    public float _maxHealth;
    public float _health;
    public float _maxMana;
    public float _mana;
    [Header("GetsugaProperties")]
    public float fireRate;
    public float simpleAttackRate;
    public Transform GetsugaTransform;
    private GetsugaManager getsugaParent;
    public UnityEngine.UI.Button getusgaButton;

    #region Start Update FixedUpdate OnDrawGizmos
    void Start()
    {
        getsugaParent = FindObjectOfType<GetsugaManager>();
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _score = 0;
    }

    
    void FixedUpdate()
    {
        var hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;

        if(!usedGetsuga)
        {
            Movement();
            Jump();
        }    

        if(_mana < _maxMana)
        {
            _mana += 5 * Time.fixedDeltaTime;
            if(_mana > _maxMana)
            {
                _mana = _maxMana;
            }
        }
    }

    private void Update()
    {
        Attack();

        CheckHealth();
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);

        Gizmos.DrawWireSphere(pointForEnemyToLookAt.position, radiusForEnemy);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(AttackCenterArea.position, AttackRadius);
    }
    #endregion 
    
    #region Attacking
    public void Attack()
    {
        if (Input.GetKey(KeyCode.F))
        {
            FireGetsuga();
        }
        if(Input.GetKey(KeyCode.R))
        {
            simpleAttack();
        }
    }
    public void simpleAttack()
    {
            if (!attacked && isGrounded)
            {
                animator.SetTrigger("isAttacking");
                StartCoroutine(simpleAttackWithTriggerReset(0.24f));
            }
    }

    IEnumerator simpleAttackWithTriggerReset(float delay)
    {
        attacked = true;
        MakeDamage(25.0f);
        yield return new WaitForSeconds(delay/2);
        animator.ResetTrigger("isAttacking");
        yield return new WaitForSeconds(delay);
        attacked = false;
    }
    public void FireGetsuga()
    {
        if(!usedGetsuga && isGrounded && _mana > 15)
        {
            animator.SetTrigger("isUsingGetsuga");
            StartCoroutine(GetsugaWithTriggerReset(fireRate));
        }
    }
    IEnumerator GetsugaWithTriggerReset(float delay)
    {
        _mana -= 15.0f;
        usedGetsuga = true;
        yield return new WaitForSeconds(delay / 2);
        var getsuga = getsugaParent.GetBullet(GetsugaTransform.position);
        usedGetsuga = false;
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger("isUsingGetsuga");
    }
    void MakeDamage(float damage)
    {
        var hit = Physics2D.OverlapCircle(AttackCenterArea.position, AttackRadius, enemyLayerMask);
        if (hit)
        {
            hit.gameObject.GetComponent<EnemyController>().health -= damage;
            Debug.Log(hit.gameObject.GetComponent<EnemyController>().health);
        }
    }
    #endregion

    #region Movement Flip Jump
    void Movement()
    {
        
        var moveX = Input.GetAxisRaw("Horizontal") + leftStick.Horizontal;
        if (moveX != 0)
        {
            Flip(moveX);
            moveX = (moveX > 0.0) ? 1.0f : -1.0f;

            _rb.AddForce(Vector2.right * moveX * horizontalForce * ((isGrounded) ? 1.0f : airFactor), ForceMode2D.Force);
            var clampedXVelocity = Mathf.Clamp(_rb.velocity.x, -horizontalSpeed, horizontalSpeed);
            _rb.velocity = new Vector2(clampedXVelocity, _rb.velocity.y);
            ChangeAnimation(PlayerAnimationState.WALK);
            
        }
        if(isGrounded && moveX == 0 && !(animationState == PlayerAnimationState.ATTACKED) && !(animationState == PlayerAnimationState.ATTACK))
        {
            ChangeAnimation(PlayerAnimationState.IDLE);
        }
        CheckState(moveX);
       
    }
    public void Flip(float x)
    {
        if (x != 0.0f)
        {
            transform.localScale = new Vector3((x > 0.0f) ? 1.0f : -1.0f, 1.0f, 1.0f);
        }
    }
    private void Jump()
    {
        var y = Input.GetAxis("Fire2") + leftStick.Vertical;


        if ((isGrounded) && (y > verticalThreshold) /*&& !(animator.GetBool("Attacking"))*/)
        {
            _rb.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
        }

        if (!isGrounded)
        {
            ChangeAnimation(PlayerAnimationState.JUMP);
        }
    }
    #endregion

    #region Animation stuff
    private void ChangeAnimation(PlayerAnimationState animState)
    {
        animationState = animState;
        animator.SetInteger("AnimationState", (int)animationState);

    }
    private IEnumerator DisableAnim(float delay, PlayerAnimationState animState)
    {
        yield return new WaitForSeconds(delay);
        ChangeAnimation(animState);
    }
    #endregion

    #region CheckHealth()/ CheckState()/ SetNewColor()
    private void CheckHealth()
    {
        if (Input.GetKey(KeyCode.H))
        {
            _health -= Time.deltaTime * 5;
        }
        if (_health <= 0)
        {
            _health = 0;
            Buttons.instance.Surrender();
        }
    }


    private void CheckState(float mX)
    {
        PlayerAnimationState anim;
        if (animationState == PlayerAnimationState.ATTACKED)
        {
            if (mX != 0)
                anim = PlayerAnimationState.WALK;
            else 
                anim = PlayerAnimationState.IDLE;
            StartCoroutine(DisableAnim(0.25f, anim));
        }
    }


    void SetNewColors(Color color, ColorBlock colorss)
    {
        ColorBlock colorsBlock = new ColorBlock();
        colorsBlock.normalColor = color;
        colorsBlock.selectedColor = color;
        colorsBlock.pressedColor = color;
        colorss = colorsBlock;
    }
    #endregion

}

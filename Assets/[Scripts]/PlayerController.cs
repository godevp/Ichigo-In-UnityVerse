using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlayerAnimationState
{
    IDLE,           //0
    WALK,           //1
    JUMP,           //2
    ATTACK,         //3
    ATTACKED        //4
}

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
    public float groundRadius; // the size of the circle
    public LayerMask groundLayerMask; // the stuff we can collide with

    private float _timer;

    [Header("Controls")]
    public Joystick leftStick;
    [Range(0.1f, 1.0f)]
    public float verticalThreshold;
    public PhysicsMaterial2D physMat;

    [Header("Sharebale")]
    public int _score;
    public float _maxHealth;
    public float _health;
    [Header("GetsugaProperties")]
    public float fireRate;
    public Transform GetsugaTransform;
    private GetsugaManager getsugaParent;


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

        Movement();
        Jump();
    }

    private void Update()
    {
        Attack();

        CheckHealth();
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
                _timer = 0;
        }
    }
    private void CheckHealth()
    {
        if (Input.GetKey(KeyCode.H))
        {
            _health -= Time.deltaTime * 5;
        }
        if(_health <= 0)
        {
            _health = 0;
            Buttons.instance.Surrender();
        }
    }

    public void Attack()
    {
        if (Input.GetKey(KeyCode.F) && _timer == 0)
        {
            FireGetsuga();
        }
        
    }
    public void FireGetsuga()
    {
        if(_timer == 0)
        {
            var getsuga = getsugaParent.GetBullet(GetsugaTransform.position);
            _timer = fireRate;
        }
    }
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


     private void Jump()
    {
        var y = Input.GetAxis("Jump") + leftStick.Vertical;

        if ((isGrounded) && (y > verticalThreshold) /*&& !(animator.GetBool("Attacking"))*/)
        {
            _rb.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
        }

        if (!isGrounded)
        {
            ChangeAnimation(PlayerAnimationState.JUMP);
        }
    }
    private void ChangeAnimation(PlayerAnimationState animState)
    {
        animationState = animState;
        animator.SetInteger("AnimationState", (int)animationState);
     
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

    private IEnumerator DisableAnim(float delay, PlayerAnimationState animState)
    {
        yield return new WaitForSeconds(delay);
        ChangeAnimation(animState);
    }


    public void Flip(float x)
    {
        if (x != 0.0f)
        {
            transform.localScale = new Vector3((x > 0.0f) ? 1.0f : -1.0f, 1.0f, 1.0f);
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }



}

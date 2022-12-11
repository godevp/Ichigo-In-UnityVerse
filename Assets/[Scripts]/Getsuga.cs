using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

/*
 
 Source file Name - Getsuga.cs
 Name - Vitaliy Karabanov
 ID - 101312885
 Date last Modified - 12/10/2022 
 Program description: getsuga it's like a magic bullet for our player, and this is the behaviour script

 */
public class Getsuga : MonoBehaviour
{
   
    private PlayerController playerController;
    private Rigidbody2D _rb;
    private GetsugaManager getsugaParent;
    public float _speed = 25.0f;
    public Boundary verticalBounds;
    public Boundary horizontalBounds;

    public float power = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        getsugaParent = FindObjectOfType<GetsugaManager>();
        playerController = FindObjectOfType<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        transform.position = playerController.GetsugaTransform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        CheckBounds();
    }
    void Movement()
    {
        if(transform.localScale.x > 0)
        {
            _rb.AddForce(Vector2.right * _speed, ForceMode2D.Impulse);
            var clampedXVelocity = Mathf.Clamp(_rb.velocity.x, _speed, _speed);
            _rb.velocity = new Vector2(clampedXVelocity, _rb.velocity.y);
        }
        if(transform.localScale.x < 0)
        {
            _rb.AddForce(Vector2.right * -_speed, ForceMode2D.Impulse);
            var clampedXVelocity = Mathf.Clamp(_rb.velocity.x, -_speed, -_speed);
            _rb.velocity = new Vector2(clampedXVelocity, _rb.velocity.y);
        }
    }

    void CheckBounds()
    {
        if ((transform.position.x > horizontalBounds.max) ||
            (transform.position.x < horizontalBounds.min) ||
            (transform.position.y > verticalBounds.max) ||
            (transform.position.y < verticalBounds.min))
        {
            getsugaParent.ReturnBullet(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyController>().health -= power;
                Destroy(this.gameObject);
            }
        }
    }

}

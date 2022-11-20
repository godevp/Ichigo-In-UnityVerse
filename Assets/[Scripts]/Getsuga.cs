using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Getsuga : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D _rb;
    [SerializeField] private float speed = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
       
        transform.localScale = new Vector3 (playerController.transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        _rb.AddForce(Vector2.right * speed , ForceMode2D.Impulse);
        var clampedXVelocity = Mathf.Clamp(_rb.velocity.x, speed, speed);
        _rb.velocity = new Vector2(clampedXVelocity, _rb.velocity.y);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private DopaminBarScript dopaminBar;
    public Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _grounded;
    private float _waitTime = 1f;
    public bool alive = true;
    private bool _animationcomplete;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive == false)
        {
            animator.SetBool("Dead", true);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        if (_grounded && alive && GetComponent<SpriteRenderer>().isVisible)
        {
            _waitTime -= Time.deltaTime;
        }

        if (_waitTime <= 0 && alive)
        {
            _waitTime = 1f;
            JumpLeft();
        }
        

        if (_animationcomplete)
        {
            Destroy(this.gameObject);
        }
    }
    
    void JumpLeft()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(new Vector2(-5f, 3f), ForceMode2D.Impulse);
        _grounded = false;
        animator.SetBool("Grounded", false);
        animator.SetBool("Jumping", true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        animator.SetBool("Grounded", true);
        animator.SetBool("Jumping", false);
        if (other.gameObject.CompareTag("Ground"))
        {
            _grounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            alive = false;
            Destroy(other.gameObject);
        }
    }
    
    public void AnimationComplete()
    {
        _animationcomplete = true;
    }
}

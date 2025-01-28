using UnityEngine;

public class EnemyScript : MonoBehaviour {
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    [SerializeField] private Animator animator;

    public Rigidbody2D rb;
    private SpriteRenderer _sr;
    private bool _grounded;
    private float _waitTime = 1f;
    public bool alive = true;
    private bool _animationcomplete;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (alive == false) {
            animator.SetBool(Dead, true);
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }

        if (_grounded && alive && GetComponent<SpriteRenderer>().isVisible) {
            _waitTime -= Time.deltaTime;
        }

        if (_waitTime <= 0 && alive) {
            _waitTime = 1f;
            JumpLeft();
        }


        if (_animationcomplete) {
            Destroy(this.gameObject);
        }
    }

    void JumpLeft() {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(-5f, 3f), ForceMode2D.Impulse);
        _grounded = false;
        animator.SetBool(Grounded, false);
        animator.SetBool(Jumping, true);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        animator.SetBool(Grounded, true);
        animator.SetBool(Jumping, false);
        if (other.gameObject.CompareTag("Ground")) {
            _grounded = true;
        }
    }

    public void AnimationComplete() {
        _animationcomplete = true;
    }
}
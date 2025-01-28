using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Dead1 = Animator.StringToHash("Dead");
    private static readonly int Grounded = Animator.StringToHash("Grounded");

    [SerializeField] private Animator animator;
    [SerializeField] private float sprintMultiplier = 1.3f;
    [SerializeField] private float sustainedJumpForce = 1.5f;
    [SerializeField] private float maxJumpTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float fallGravity;
    [SerializeField] private AudioSource audioSourceJump;
    [SerializeField] private AudioClip soundEffectJump;
    [SerializeField] private AudioSource audioSourceLand;
    [SerializeField] private AudioClip soundEffectLand;

    [Header("Movement")] [SerializeField] private float movementSpeed = 7.5f;
    [Header("Jumping")] [SerializeField] private float jumpForce = 10;
    [Header("For Camera")] public bool isAlive = true;

    public bool stopCamera;
    [HideInInspector] public int jetons;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _isGrounded = true;
    private bool _isJumping;
    private Vector2 _targetVelocity;
    private float _currentSpeed;
    private float _jumpTimeCounter;
    private float _coyoteTimeCounter;
    private bool _startCoyoteTime;
    private float _deathTime = 2f;
    private bool _finished;
    private bool _finishedMove;
    private float _finishedTime = 1f;
    private bool _sprinting;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _coyoteTimeCounter = coyoteTime;
        jetons = 40000;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") < 0 && !_finished) {
            _sr.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") > 0 && !_finished) {
            _sr.flipX = false;
        }

        if (_isGrounded) {
            _coyoteTimeCounter = coyoteTime;
        }
        else {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (_rb.linearVelocity.x != 0) {
            animator.SetFloat(Speed, Mathf.Abs(_rb.linearVelocity.x));
        }

        if (!_isGrounded && _rb.linearVelocity.y < 0) {
            animator.SetBool(Jumping, false);
        }

        if (_coyoteTimeCounter >= 0) {
            Jump();
        }

        if (_finished) {
            _finishedTime -= Time.deltaTime;
            if (_finishedTime <= 0) {
                _finishedMove = true;
                Move();
            }
        }
    }

    private void FixedUpdate() {
        if (isAlive && !_finished) {
            Move();
        }
        else if (!isAlive && !_finished) {
            animator.SetBool(Dead1, true);
            Dead();
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            if (Mathf.RoundToInt(_rb.linearVelocity.y) != 0) {
                return;
            }
            _isGrounded = true;
            animator.SetBool(Grounded, true);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            _isGrounded = false;
            animator.SetBool(Grounded, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            audioSourceLand.PlayOneShot(soundEffectLand);
        }

        if (other.gameObject.CompareTag("Enemy")) {
            _rb.linearVelocity = Vector2.zero;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Vector2.up.y * 10);
            isAlive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            _rb.linearVelocity = Vector2.zero;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Vector2.up.y * 10);
            isAlive = false;
        }
        else if (other.gameObject.CompareTag("EnemyHead")) {
            EnemyScript enemy = other.gameObject.GetComponentInParent<EnemyScript>();
            enemy.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            enemy.alive = false;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 10f);
        }
        else if (other.gameObject.CompareTag("StopCamera")) {
            stopCamera = true;
            if (_finished) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (other.gameObject.CompareTag("Finish")) {
            Debug.Log("Game Over");
            _finished = true;
            _rb.linearVelocity = new Vector2(0, 0);
            stopCamera = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("StopCamera")) {
            stopCamera = false;
        }
    }

    private void Move() {
        if (_finishedMove) {
            _rb.linearVelocity = new Vector2(movementSpeed * 1, _rb.linearVelocity.y);
        }

        if (!_finished) {
            if (Input.GetKey(KeyCode.LeftShift) && _isGrounded) {
                _sprinting = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift)) {
                _sprinting = false;
            }

            if (_sprinting) {
                _rb.linearVelocity = new Vector2(movementSpeed * sprintMultiplier * Input.GetAxisRaw("Horizontal"),
                    _rb.linearVelocity.y);
            }
            else {
                _rb.linearVelocity = new Vector2(movementSpeed * Input.GetAxisRaw("Horizontal"), _rb.linearVelocity.y);
            }
        }
    }

    private void Jump() {
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space) ||
            (_coyoteTimeCounter >= 0 && Input.GetKeyDown(KeyCode.Space))) {
            animator.SetBool(Jumping, true);
            _isJumping = true;
            _jumpTimeCounter = maxJumpTime;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _isGrounded = false;
        }

        if (_isJumping && Input.GetKeyDown(KeyCode.Space)) {
            audioSourceJump.PlayOneShot(soundEffectJump);
            if (_jumpTimeCounter >= 0) {
                _rb.AddForce(Vector2.up * sustainedJumpForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else {
                _isJumping = false;
                _coyoteTimeCounter = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            _isJumping = false;
            _coyoteTimeCounter = 0;
        }

        if (!_isJumping && !_isGrounded) {
            _rb.AddForce(Vector2.down * fallGravity);
        }
    }

    private void Dead() {
        _rb.GetComponent<Collider2D>().enabled = false;
        _deathTime -= Time.deltaTime;
        _rb.freezeRotation = false;
        _rb.AddTorque(-20f);

        if (_deathTime <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
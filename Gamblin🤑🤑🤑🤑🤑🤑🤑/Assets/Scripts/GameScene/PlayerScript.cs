using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene {
    public class PlayerScript : MonoBehaviour {
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Dead1 = Animator.StringToHash("Dead");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Throwing = Animator.StringToHash("Throwing");

    [SerializeField] private Animator animator;

    [Header("Movement")] public float movementSpeed = 7.5f;
    [SerializeField] private float sprintMultiplier = 1.3f;
    public float maxSprintTime = float.MaxValue;
    private bool _isWaitingSprint;

    [Header("Jumping")] public float jumpForce = 10;
    [SerializeField] private float sustainedJumpForce = 1.5f;
    [SerializeField] private float maxJumpTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float fallGravity;
    public int maxJumps = int.MaxValue;
    private bool _isWaitingJumps;

    [Header("Shooting")] [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed = 12.5f;
    [SerializeField] private float projectileCoolDown = 3f;

    [Header("For Camera")] public bool isAlive = true;
    public bool stopCamera;

    [Header("Dopamine Bar")] [SerializeField]
    [HideInInspector]public DopaminBarScript dopaminBar;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSourceJump;
    [SerializeField] private AudioClip soundEffectJump;
    [SerializeField] private AudioSource audioSourceLand;
    [SerializeField] private AudioClip soundEffectLand;
    [SerializeField] private AudioSource audioSourceWalking;
    [SerializeField] private AudioClip soundEffectWalking;
    [SerializeField] private AudioSource dopamineRegainSource;
    [SerializeField] private AudioClip dopamineRegainClip;


    [HideInInspector] public int jetons;
    [HideInInspector] public float time;

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
    private Transform _head;
    private int _lookingAsInt = 1;
    private float _coolDownValue;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _coyoteTimeCounter = coyoteTime;
        jetons = 40000;
        _head = GetComponentInChildren<Transform>();
        _coolDownValue = projectileCoolDown;
        projectileCoolDown = 0;
        time = 10f;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") < 0 && !_finished) {
            _sr.flipX = true;
            _lookingAsInt = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") > 0 && !_finished) {
            _sr.flipX = false;
            _lookingAsInt = 1;
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

        if (_coyoteTimeCounter >= 0 && isAlive) {
            Jump();
        }

        if (_finished) {
            _finishedTime -= Time.deltaTime;
            if (_finishedTime <= 0) {
                _finishedMove = true;
                Move();
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && projectileCoolDown <= 0 && isAlive && !_finished) {
            Shoot();
            animator.SetBool(Throwing, true);
            projectileCoolDown = _coolDownValue;
        }
        else {
            projectileCoolDown -= Time.deltaTime;
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
        if(!dopaminBar.waiting) time -= Time.fixedDeltaTime;
        if (time <= 0) time = 0;
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
            // EnemyScript enemy = other.gameObject.GetComponentInParent<EnemyScript>();
            // enemy.rb.GetComponentInChildren<Collider2D>().enabled = false;
            _rb.linearVelocity = Vector2.zero;
            isAlive = false;
        }
        else if (other.gameObject.CompareTag("EnemyHead")) {
            if (other.gameObject.GetComponentInParent<JetonScript>() != null)
            {
                JetonScript jeton = other.gameObject.GetComponentInParent<JetonScript>();
                jeton.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                jeton.alive = false;
            }
            else
            {
                DiceScript dice = other.gameObject.GetComponentInParent<DiceScript>();
                dice.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                dice.alive = false;
            }
            dopamineRegainSource.PlayOneShot(dopamineRegainClip);
            dopaminBar.time += 1.5f;
            time += 1.5f;
            if(dopaminBar.time >= 10) dopaminBar.time = 10;
            if(time >= 10) time = 10;
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
                if (maxSprintTime <= 0) {
                    if(!_isWaitingSprint) StartCoroutine(ResetSprint());
                    _sprinting = false;
                }
                if(_sprinting && maxSprintTime >= 0) maxSprintTime -= Time.deltaTime;
                else maxSprintTime = 0;
            }
            else if (!Input.GetKey(KeyCode.LeftShift)) {
                _sprinting = false;
            }

            if (_sprinting) {
                if(!audioSourceWalking.isPlaying && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))) audioSourceWalking.PlayOneShot(soundEffectWalking);
                _rb.linearVelocity = new Vector2(movementSpeed * sprintMultiplier * Input.GetAxisRaw("Horizontal"), _rb.linearVelocity.y);
            }
            else {
                if(!audioSourceWalking.isPlaying && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))) audioSourceWalking.PlayOneShot(soundEffectWalking);
                _rb.linearVelocity = new Vector2(movementSpeed * Input.GetAxisRaw("Horizontal"), _rb.linearVelocity.y);
            }
        }
    }

    private void Jump() {
        if (_isWaitingJumps) return;
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space) ||
            (_coyoteTimeCounter >= 0 && Input.GetKeyDown(KeyCode.Space))) {
            maxJumps--;
            if (maxJumps <= 0) {
                maxJumps = 0;
                StartCoroutine(ResetJumps());
            }
            animator.SetBool(Jumping, true);
            _isJumping = true;
            _jumpTimeCounter = maxJumpTime;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _isGrounded = false;
        }

        if (_isJumping && Input.GetKey(KeyCode.Space)) {
            if (_jumpTimeCounter >= 0) {
                _rb.AddForce(Vector2.up * sustainedJumpForce);
                _jumpTimeCounter -= Time.deltaTime;
            }
            else {
                _isJumping = false;
                _coyoteTimeCounter = 0;
            }
        }

        if (_isJumping && Input.GetKeyDown(KeyCode.Space)) {
            audioSourceJump.PlayOneShot(soundEffectJump);
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
        _rb.GetComponent<Collider2D>();
        _deathTime -= Time.deltaTime;

        if (_deathTime <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private bool CheckForWall() {
        Vector2 rayDirection = new Vector2(Mathf.Sign(_rb.linearVelocity.x), 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 0.5f, LayerMask.GetMask("Ground"));

        if (hit.collider is not null) {
            return true;
        }
        return false;
    }

    public void Shoot() {
        animator.Play("Throw");
        GameObject projectile = GameObject.Instantiate(this.projectile, _head.position, _head.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddTorque(-1000f);
        projectileRb.linearVelocity = new Vector2(projectileSpeed * _lookingAsInt, _rb.linearVelocity.y);
    }

    private IEnumerator ResetJumps() {
        _isWaitingJumps = true;
        yield return new WaitForSeconds(5f);
        maxJumps = 3;
        _isWaitingJumps = false;
    }

    private IEnumerator ResetSprint() {
        _isWaitingSprint = true;
        yield return new WaitForSeconds(5f);
        maxSprintTime = 5f;
        _isWaitingSprint = false;
    }

    public void StopThrow()
    {
        animator.SetBool(Throwing, false);
    }
}
}
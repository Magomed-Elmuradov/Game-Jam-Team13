using UnityEngine;

namespace GameScene {
    public class JetonScript : MonoBehaviour {
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        [SerializeField] private Animator animator;
        [SerializeField] private DopaminBarScript dopaminBar;
        [SerializeField] private float speed;
        [SerializeField] private AudioSource rollingSource;
        [SerializeField] private AudioClip rollingClip;

        public Rigidbody2D rb;
        private SpriteRenderer _sr;
        internal bool _grounded;
        internal float _waitTime = 1f;
        public bool alive = true;
        private bool _animationcomplete;

        void Start() {
            rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            rollingSource = GameObject.Find("JetonRollingSource").GetComponent<AudioSource>();
            rollingClip = Resources.Load<AudioClip>("Sounds/jetonRolling");
        }

        void Update() {
            if (alive == false) {
                animator.SetBool(Dead, true);
                foreach (Transform child in transform) {
                    Destroy(child.gameObject);
                }
            }

            if (_animationcomplete) {
                Destroy(gameObject);
            }
        }

        void FixedUpdate() {
            if (alive && rb.linearVelocityX > -10 && _sr.isVisible) {
                MoveLeft();
            }
        }

        private void MoveLeft() {
            if(!rollingSource.isPlaying) rollingSource.PlayOneShot(rollingClip);
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }

        internal void JumpLeft() {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(-5f * speed, 3f * speed), ForceMode2D.Impulse);
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

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Projectile")) {
                ProjectileScript projectile = other.gameObject.GetComponent<ProjectileScript>();
                projectile.Shatter();
                alive = false;
            }
        }

        public void AnimationComplete() {
            _animationcomplete = true;
        }
    }
}
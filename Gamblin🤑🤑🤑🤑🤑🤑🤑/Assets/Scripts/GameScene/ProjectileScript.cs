using UnityEngine;

namespace GameScene {
    public class ProjectileScript : MonoBehaviour {
        private static readonly int Shatter1 = Animator.StringToHash("Shatter");

        [SerializeField] private DopaminBarScript dopaminBar;
        [SerializeField] private PlayerScript player;
        [SerializeField] private AudioSource dopamineRegainSource;
        [SerializeField] private AudioClip dopamineRegainClip;
        [SerializeField] private Animator anim;
        [SerializeField] private AudioSource shatterSource;
        [SerializeField] private AudioClip shatterClip;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;

        void Start() {
            _sr = GetComponent<SpriteRenderer>();
            dopaminBar = GameObject.Find("DopamineBar").GetComponent<DopaminBarScript>();
            player = GameObject.Find("Player").GetComponent<PlayerScript>();
            dopamineRegainSource = GameObject.Find("GainDopamineSource").GetComponent<AudioSource>();
            dopamineRegainClip = Resources.Load<AudioClip>("Sounds/dopaminRegain");
            shatterSource = GameObject.Find("GlassBreakingSource").GetComponent<AudioSource>();
            shatterClip = Resources.Load<AudioClip>("Sounds/glassBreaking");
            anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update() {
            if (_sr.isVisible == false) {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyHead")) {
                dopamineRegainSource.PlayOneShot(dopamineRegainClip);
                dopaminBar.time += 1.5f;
                player.time += 1.5f;
                if(dopaminBar.time >= 10) dopaminBar.time = 10;
                if(player.time >= 10) player.time = 10;
                _rb.simulated = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Ground")) {
                Shatter();
            }
        }

        public void Shatter() {
            shatterSource.PlayOneShot(shatterClip);
            anim.SetBool(Shatter1, true);
            _rb.linearVelocity = Vector2.zero;
            _rb.AddTorque(20);
            _rb.freezeRotation = true;
            _rb.constraints = RigidbodyConstraints2D.None;
        }

        public void DestroyBottle() {
            Destroy(gameObject);
        }
    }
}
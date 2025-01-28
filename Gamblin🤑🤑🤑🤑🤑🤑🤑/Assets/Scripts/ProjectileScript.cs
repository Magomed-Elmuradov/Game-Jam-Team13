using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private DopaminBarScript dopaminBar;
    [SerializeField] private PlayerScript player;
    [SerializeField] private AudioSource dopamineRegainSource;
    [SerializeField] private AudioClip dopamineRegainClip;
    private SpriteRenderer _sr;
    
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        dopaminBar = GameObject.Find("DopamineBar").GetComponent<DopaminBarScript>();
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        dopamineRegainSource = GameObject.Find("GainDopamineSource").GetComponent<AudioSource>();
        dopamineRegainClip = Resources.Load<AudioClip>("Sounds/dopaminRegain");
    }

    // Update is called once per frame
    void Update()
    {
        if (_sr.isVisible == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyHead")) {
            dopamineRegainSource.PlayOneShot(dopamineRegainClip);
            dopaminBar.time += 3;
            player.time += 3;
        }
    }
}

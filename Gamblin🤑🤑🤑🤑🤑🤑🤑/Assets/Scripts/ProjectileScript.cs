using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private DopaminBarScript dopaminBar;
    private SpriteRenderer _sr;
    
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        dopaminBar = GameObject.Find("DopamineBar").GetComponent<DopaminBarScript>();
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
            dopaminBar.time += 3;
        }
    }
}

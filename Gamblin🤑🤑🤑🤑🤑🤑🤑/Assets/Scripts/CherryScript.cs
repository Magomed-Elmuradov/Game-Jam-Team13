using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryScript : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private bool animationComplete;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animationComplete)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.cherryCount += 1;
        animator.SetBool("Collected", true);
    }

    public void AnimationComplete()
    {
        animationComplete = true;
    }
}

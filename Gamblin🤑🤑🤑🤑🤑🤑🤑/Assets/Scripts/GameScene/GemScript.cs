using UnityEngine;

namespace GameScene {
    public class GemScript : MonoBehaviour {
        private static readonly int Collected = Animator.StringToHash("Collected");
        private bool _animationComplete;
        [SerializeField] private Animator animator;

        void Update() {
            if (_animationComplete) {
                Destroy(this.gameObject);
            }
        }

        public void OnTriggerEnter2D(Collider2D other) {
            animator.SetBool(Collected, true);
        }

        public void AnimationComplete() {
            _animationComplete = true;
        }
    }
}
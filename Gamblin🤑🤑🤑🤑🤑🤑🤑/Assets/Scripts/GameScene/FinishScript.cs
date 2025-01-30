using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene {
    public class FinishScript : MonoBehaviour {
        private static readonly int Collected = Animator.StringToHash("Collected");
        private bool _animationComplete;
        [SerializeField] private Animator animator;

        void Update() {
            if (_animationComplete) {
                SceneManager.LoadScene("StartScene");
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
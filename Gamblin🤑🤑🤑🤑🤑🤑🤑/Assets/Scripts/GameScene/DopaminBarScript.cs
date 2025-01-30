using UnityEngine;
using UnityEngine.UI;

namespace GameScene {
    public class DopaminBarScript : MonoBehaviour {
        [SerializeField] public Slider slider;
        [SerializeField] public Image fillImage;
        [SerializeField] public float time = 10f;
        [SerializeField] public PlayerScript player;
        [HideInInspector] public bool waiting;
        [SerializeField] private AudioSource audioSourceLowDopamine;
        [SerializeField] private AudioClip soundEffectLowDopamine;
        private readonly Color _green = new Color32(28, 231, 64, 255);

        void Update() {
            if (!waiting) {
                slider.value = time;
                time -= Time.deltaTime;
            }

            if (time > 10) {
                time = 10;
                slider.value = time;
            }

            if (slider.value <= 2.5) {
                Flash();
                if (!audioSourceLowDopamine.isPlaying) audioSourceLowDopamine.PlayOneShot(soundEffectLowDopamine);
            }
            else if (audioSourceLowDopamine.isPlaying) audioSourceLowDopamine.Stop();
            else fillImage.color = _green;

            if (slider.value <= 0) player.isAlive = false;
        }

        private void Flash() {
            fillImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 0.3f));
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScene {
    public class SellingScript : MonoBehaviour {
        [SerializeField] private Button eyeSellButton;
        [SerializeField] private Button legSellButton;
        [SerializeField] private Button lungSellButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button menuButton;
        private TMP_Text _eyeSellText;
        private TMP_Text _legSellText;
        private TMP_Text _lungSellText;
        [SerializeField] private PlayerScript player;
        [SerializeField] private Volume postProcessingVolume;
        private Vignette _vignette;
        public GameObject sellingOverlay;
        private static bool _isOverlayActive;

        void Start() {
            eyeSellButton.onClick.AddListener(SellEye);
            legSellButton.onClick.AddListener(SellLeg);
            lungSellButton.onClick.AddListener(SellLung);
            closeButton.onClick.AddListener(ToggleOverlay);
            menuButton.onClick.AddListener(BackToMenu);
            _eyeSellText = eyeSellButton.GetComponentInChildren<TMP_Text>();
            _legSellText = legSellButton.GetComponentInChildren<TMP_Text>();
            _lungSellText = lungSellButton.GetComponentInChildren<TMP_Text>();
            sellingOverlay.SetActive(false);
            _isOverlayActive = false;
            if (postProcessingVolume.profile.TryGet(out _vignette)) _vignette.intensity.value = 0f;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) ToggleOverlay();
            if (Input.GetKeyDown(KeyCode.Q) && player.jetons < 20000) ToggleOverlay();
        }

        private void ToggleOverlay() {
            _isOverlayActive = !_isOverlayActive;
            sellingOverlay.SetActive(_isOverlayActive);
            Time.timeScale = _isOverlayActive ? 0 : 1;
        }

        void SellEye() {
            player.jetons += 60000;
            eyeSellButton.enabled = false;
            if (_vignette != null) _vignette.intensity.value = 0.8f;
            _eyeSellText.text = "Sold";
        }

        void SellLeg() {
            player.jetons += 60000;
            player.movementSpeed *= 0.5f;
            player.jumpForce *= 0.7f;
            legSellButton.enabled = false;
            _legSellText.text = "Sold";
        }

        void SellLung() {
            player.jetons += 60000;
            player.maxJumps = 3;
            player.maxSprintTime = 5f;
            lungSellButton.enabled = false;
            _lungSellText.text = "Sold";
        }

        void BackToMenu() {
            SceneManager.LoadScene("StartScene");
        }
    }
}
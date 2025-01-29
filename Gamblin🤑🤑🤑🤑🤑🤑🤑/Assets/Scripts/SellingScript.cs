using UnityEngine;
using UnityEngine.UI;

public class SellingScript : MonoBehaviour {
    [SerializeField] private Button eyeSellButton;
    [SerializeField] private Button legSellButton;
    [SerializeField] private Button lungSellButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private PlayerScript player;
    public GameObject sellingOverlay;
    private static bool _isOverlayActive;
    private bool _overlayClosed;

    void Start() {
        eyeSellButton.onClick.AddListener(SellEye);
        legSellButton.onClick.AddListener(SellLeg);
        lungSellButton.onClick.AddListener(SellLung);
        closeButton.onClick.AddListener(ToggleOverlay);
        sellingOverlay.SetActive(false);
        _isOverlayActive = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) ToggleOverlay();
    }

    private void ToggleOverlay() {
        _overlayClosed = !_overlayClosed;
        _isOverlayActive = !_isOverlayActive;
        sellingOverlay.SetActive(_isOverlayActive);

        if (_isOverlayActive) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    void SellEye() {
        player.jetons += 60000;
        eyeSellButton.enabled = false;
    }

    void SellLeg() {
        player.jetons += 60000;
        player.movementSpeed *= 0.5f;
        player.jumpForce *= 0.7f;
        legSellButton.enabled = false;
    }

    void SellLung() {
        player.jetons += 60000;
        lungSellButton.enabled = false;
    }
}
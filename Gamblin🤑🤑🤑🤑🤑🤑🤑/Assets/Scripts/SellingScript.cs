using UnityEngine;
using UnityEngine.UI;

public class SellingScript : MonoBehaviour {
    [SerializeField] private Button eyeSellButton;
    [SerializeField] private Button legSellButton;
    [SerializeField] private Button lungSellButton;
    [SerializeField] private PlayerScript player;

    void Start() {
        eyeSellButton.onClick.AddListener(SellEye);
        legSellButton.onClick.AddListener(SellLeg);
        lungSellButton.onClick.AddListener(SellLung);
    }

    void SellEye() {
        player.jetons += 60000;
    }

    void SellLeg() {
        player.jetons += 60000;
        player.movementSpeed *= 0.5f;
        player.jumpForce *= 0.7f;
    }

    void SellLung() {
        player.jetons += 60000;
    }
}
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private TMP_Text jetonText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private PlayerScript player;

    void Update() {
        jetonText.text = $"Jetons: {player.jetons}";
        timerText.text = $"{player.time:F1}";
    }
}
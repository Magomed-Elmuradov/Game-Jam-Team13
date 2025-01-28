using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private TMP_Text jetonText;
    [SerializeField] private PlayerScript player;

    void Update() {
        jetonText.text = $"Time: {player.time:F1}\nJetons: {player.jetons}";
    }
}
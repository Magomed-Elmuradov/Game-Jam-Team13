using TMPro;
using UnityEngine;

namespace GameScene {
    public class GameManager : MonoBehaviour {
        [SerializeField] private TMP_Text jetonText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private PlayerScript player;

        void Update() {
            if (player.maxJumps < 10)
                jetonText.text =
                    $"Jetons: {player.jetons}\nMax Jumps: {player.maxJumps}\nMax Sprint: {player.maxSprintTime:F1}";
            else jetonText.text = $"Jetons: {player.jetons}";
            timerText.text = $"{player.time:F1}";
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChooseLevelScene {
    public class ChooseLevelScript : MonoBehaviour {
        [SerializeField] private Button menuButton;
        [SerializeField] private Button level1Button;
        [SerializeField] private Button level2Button;

        void Start() {
            menuButton.onClick.AddListener(BackToMenu);
            level1Button.onClick.AddListener(Level1);
            level2Button.onClick.AddListener(Level2);
        }

        void BackToMenu() {
            SceneManager.LoadScene("StartScene");
        }

        void Level1() {
            SceneManager.LoadScene("Level1");
        }

        void Level2() {
            SceneManager.LoadScene("Level2");
        }
    }
}
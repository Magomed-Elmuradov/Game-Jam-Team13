using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StartScene {
    public class StartSceneController : MonoBehaviour {
        public Button startButton;
        public Button exitButton;
        public Button creditsButton;
        public Button controlsButton;

        void Start() {
            startButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(ExitGame);
            creditsButton.onClick.AddListener(Credits);
            controlsButton.onClick.AddListener(Controls);
        }

        void StartGame() {
            SceneManager.LoadScene("ChooseLevelScene");
        }

        void ExitGame() {
            Application.Quit();
        }

        void Credits() {
            SceneManager.LoadScene("CreditScene");
        }

        void Controls() {
            SceneManager.LoadScene("ControlsScene");
        }
    }
}
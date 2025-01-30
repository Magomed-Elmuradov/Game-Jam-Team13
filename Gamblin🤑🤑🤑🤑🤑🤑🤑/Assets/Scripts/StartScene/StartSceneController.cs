using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StartScene {
    public class StartSceneController : MonoBehaviour {
        public Button startButton;
        public Button exitButton;
        public Button creditsButton;

        void Start() {
            startButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(ExitGame);
            creditsButton.onClick.AddListener(Credits);
        }

        void StartGame() {
            SceneManager.LoadScene("Level1");
        }

        void ExitGame() {
            Application.Quit();
        }

        void Credits() {
            SceneManager.LoadScene("CreditScene");
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StartScene {
    public class StartSceneController : MonoBehaviour {
        public Button startButton;
        public Button exitButton;

        void Start() {
            startButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(ExitGame);
        }

        void StartGame() {
            SceneManager.LoadScene("Level1");
        }

        void ExitGame() {
            Application.Quit();
        }
    }
}
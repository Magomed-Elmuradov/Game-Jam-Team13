using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ControlsScene {
    public class ControlsController : MonoBehaviour {
        [SerializeField] private Button menuButton;

        void Start() {
            menuButton.onClick.AddListener(BackToMenu);
        }

        void BackToMenu() {
            SceneManager.LoadScene("StartScene");
        }
    }
}
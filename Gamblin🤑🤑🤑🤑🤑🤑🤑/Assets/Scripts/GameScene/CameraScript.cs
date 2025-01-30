using Unity.Cinemachine;
using UnityEngine;

namespace GameScene {
    public class CameraScript : MonoBehaviour {
        [SerializeField] private CinemachineCamera vcam;
        [SerializeField] private PlayerScript player;

        void Update() {
            if (player.isAlive == false || player.stopCamera) {
                vcam.Follow = null;
            }
            else {
                vcam.Follow = player.transform;
            }
        }
    }
}
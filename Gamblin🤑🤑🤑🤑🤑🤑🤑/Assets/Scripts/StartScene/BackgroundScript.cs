using UnityEngine;

namespace StartScene {
    public class EndlessScroll : MonoBehaviour {
        public float scrollSpeed = 2f;
        private float _spriteWidth;
        private Transform[] _layers;

        void Start() {
            _spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
            _layers = new Transform[transform.parent.childCount];

            for (int i = 0; i < transform.parent.childCount; i++) {
                _layers[i] = transform.parent.GetChild(i);
            }
        }

        void Update() {
            foreach (Transform layer in _layers) {
                layer.position += Vector3.left * (scrollSpeed * Time.deltaTime);
            }

            foreach (Transform layer in _layers) {
                if (layer.position.x <= -_spriteWidth) {
                    float rightmostX = GetRightmostX();
                    layer.position = new Vector3(rightmostX + _spriteWidth, layer.position.y, layer.position.z);
                }
            }
        }

        float GetRightmostX() {
            float maxX = float.MinValue;
            foreach (Transform layer in _layers) {
                if (layer.position.x > maxX) {
                    maxX = layer.position.x;
                }
            }

            return maxX;
        }
    }
}

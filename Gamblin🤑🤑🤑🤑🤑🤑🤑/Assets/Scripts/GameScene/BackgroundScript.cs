using UnityEngine;

namespace GameScene {
    public class BackgroundScript : MonoBehaviour {
        public float parallaxSpeed;
        private float _length, _startPos;
        private Transform _cam;

        private void Start() {
            if (Camera.main != null) _cam = Camera.main.transform;
            _startPos = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update() {
            var distance = _cam.transform.position.x * parallaxSpeed;
            transform.position = new Vector3(_startPos + distance, transform.position.y, transform.position.z);

            if (transform.position.x > _startPos + _length) _startPos += _length;
            else if (transform.position.x < _startPos - _length) _startPos -= _length;
        }
    }
}
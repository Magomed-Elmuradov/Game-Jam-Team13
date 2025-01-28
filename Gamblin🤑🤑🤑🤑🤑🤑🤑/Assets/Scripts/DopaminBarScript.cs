using UnityEngine;
using UnityEngine.UI;

public class DopaminBarScript : MonoBehaviour {
    [SerializeField] public Slider slider;
    [SerializeField] public Image fillImage;
    [SerializeField] public float time = 20f;
    [SerializeField] public PlayerScript player;
    [HideInInspector] public bool waiting;
    private readonly Color _green = new Color32(28, 231, 64, 255);

    void Update() {
        if (!waiting) {
            slider.value = time;
            time -= Time.deltaTime;
        } else slider.value = 20f;

        if (time > 20)
        {
            time = 20;
        }
        
        if (slider.value <= 5) Flash();
        else fillImage.color = _green;

        if (slider.value <= 0) player.isAlive = false;
    }

    private void Flash() {
        fillImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
    }

    public void Reset() {
        waiting = true;
        time = 20f;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class DopaminBarScript : MonoBehaviour {
    [SerializeField] public Slider slider;
    [SerializeField] public Image fillImage;
    [SerializeField] public float time = 20f;
    [SerializeField] public float flashSpeed = 1f;
    [SerializeField] public PlayerScript player;
    [HideInInspector] public bool waiting;
    private readonly Color _green = new Color32(28, 231, 64, 255);

    void Update() {
        if (!waiting) {
            slider.value = time;
            time -= Time.deltaTime;
        } else slider.value = 20f;

        if (slider.value <= 5) Flash();
        else fillImage.color = _green;

        if (slider.value <= 0) player.isAlive = false;
    }

    private void Flash() {
        //t = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));
        fillImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
    }

    public void Reset() {
        waiting = true;
    }
}
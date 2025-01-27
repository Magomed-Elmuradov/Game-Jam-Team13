using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DopaminBarScript : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] public Image fillImage;
    [SerializeField] public float time = 20f;
    [SerializeField] public float flashSpeed = 1f;
    [SerializeField] public PlayerScript player;
    [HideInInspector] public bool waiting;

    void Update()
    {
        if (!waiting) {
            slider.value = time;
            time -= Time.deltaTime;
        }

        if (slider.value <= 5)
        {
            Flash();
        }

        if (slider.value <= 0)
        {
            player.isAlive = false;
        }
    }

    public void Flash()
    {
        //t = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));
        fillImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
    }

    public void Reset() {
        time = 20f;
    }
}

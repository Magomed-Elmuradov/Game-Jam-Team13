using UnityEngine;
using UnityEngine.UI;

public class DopaminBarScript : MonoBehaviour
{
    [SerializeField]
    public Slider slider;

    [SerializeField] 
    public float time = 20f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = time;
        time -= Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour {
    [SerializeField] private TMP_Text jetonText;
    [SerializeField] private PlayerScript player;

    void Update() {
        jetonText.text = $"Jetons: {player.jetons}";
    }
}
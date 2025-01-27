using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SlotMachine : MonoBehaviour {
    private Dictionary<int, int> _outcomes;
    private int _lastResult;
    private bool _inputLocked;
    private float _timeRemaining;
    private bool _gamblingAnim;

    [SerializeField] private TMP_Text uiTimer;

    void Start() {
        _outcomes = new Dictionary<int, int> {
            { 40000, 5 }, // 5% Chance
            { 30000, 10 }, // 10% Chance
            { 20000, 25 }, // 25% Chance
            { 10000, 30 }, // 30% Chance
            { 5000, 20 }, // 20% Chance
            { 0, 10 } // 10% Chance
        };
       uiTimer.text = "GAMBLE   !";
    }

    int CalculateWeightedOutcome(Dictionary<int, int> weightedOutcomes) {
        int randomValue = Random.Range(0, 100);
        int cumulativeWeight = 0;

        foreach (var kvp in weightedOutcomes) {
            cumulativeWeight += kvp.Value;
            if (randomValue < cumulativeWeight) {
                _lastResult = kvp.Key;
                return kvp.Key;
            }
        }
        return 0;
    }

    void Update() {
        if (_inputLocked) {
            if (_timeRemaining > 0) {
                _timeRemaining -= Time.deltaTime;
                if(!_gamblingAnim) uiTimer.text = $"Letzter Gewinn: {_lastResult}\nTime Remaining: {Mathf.CeilToInt(_timeRemaining)}s";
            } else {
                _inputLocked = false;
                uiTimer.text = $"Letzter Gewinn: {_lastResult}\nTime to Gamble again!";
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            CalculateWeightedOutcome(_outcomes);
            StartCoroutine(Gambling());
            _timeRemaining = 8f;
            _inputLocked = true;
        }
    }

    private IEnumerator Gambling() {
        var startTime = Time.time;
        _gamblingAnim = true;
        while (Time.time - startTime < 8f) {
            var result =_outcomes.ElementAt(Random.Range(0, _outcomes.Count)).Key;
            uiTimer.text = $"Letzter Gewinn: {result}\nTime Remaining: {Mathf.CeilToInt(_timeRemaining)}s";
            yield return new WaitForSeconds(0.1f);
        }
        _gamblingAnim = false;
        _inputLocked = false;
    }
}
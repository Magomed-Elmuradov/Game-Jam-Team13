using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameScene {
    public class SlotMachineScript : MonoBehaviour {
        private Dictionary<int, int> _outcomes;
        private int _lastResult;
        private bool _inputLocked;
        private float _timeRemaining;
        private bool _gamblingAnim;

        [SerializeField] private PlayerScript player;
        [SerializeField] private AudioSource audioSourceWin;
        [SerializeField] private AudioClip soundEffectWin;
        [SerializeField] private AudioSource audioSourceLoose;
        [SerializeField] private AudioClip soundEffectLoose;
        [SerializeField] private TMP_Text gamblingText;
        [SerializeField] private DopaminBarScript dopaminBar;
        [HideInInspector] public bool _isActive;

        void Start() {
            _outcomes = new Dictionary<int, int> {
                { 40000, 1 }, // 1% Chance
                { 30000, 5 }, // 5% Chance
                { 20000, 10 }, // 10% Chance
                { 10000, 40 }, // 40% Chance
                { 5000, 20 }, // 20% Chance
                { 0, 24 } // 24% Chance
            };
            gamblingText.text = "Press Q to \nGAMBLE!";
        }

        int CalculateWeightedOutcome(Dictionary<int, int> weightedOutcomes) {
            int randomValue = Random.Range(0, 100);
            int cumulativeWeight = 0;

            foreach (var kvp in weightedOutcomes) {
                cumulativeWeight += kvp.Value;
                if (randomValue < cumulativeWeight) {
                    return kvp.Key;
                }
            }

            return 0;
        }

        void Update() {
            if (_inputLocked) {
                if (_timeRemaining > 0) {
                    _timeRemaining -= Time.deltaTime;
                    if (!_gamblingAnim)
                        gamblingText.text =
                            $"Last Win: {_lastResult}\nTime Remaining: {Mathf.CeilToInt(_timeRemaining)}s";
                }
                else {
                    _inputLocked = false;
                    gamblingText.text = $"Last Win: {_lastResult}\nTime to Gamble again!";
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                if (!player.isAlive) return;
                if (player.jetons < 20000) {
                    gamblingText.text = $"Last Win: {_lastResult}\nInsufficient Jetons!\nMake some Money!";
                    return;
                }

                player.jetons -= 20000;
                _lastResult = CalculateWeightedOutcome(_outcomes);
                StartCoroutine(Gambling());
                StartCoroutine(FillSyringe());
                if (_lastResult < 20000) audioSourceLoose.PlayOneShot(soundEffectLoose);
                else audioSourceWin.PlayOneShot(soundEffectWin);
                _timeRemaining = 5.5f;
                _inputLocked = true;
            }
        }

        private IEnumerator Gambling() {
            _isActive = true;
            var startTime = Time.time;
            _gamblingAnim = true;
            yield return new WaitForSeconds(1.3f);
            while (Time.time - startTime < 5.5f) {
                var result = _outcomes.ElementAt(Random.Range(0, _outcomes.Count)).Key;
                gamblingText.text = $"Last Win: {result}\nTime Remaining: {Mathf.CeilToInt(_timeRemaining)}s";
                yield return new WaitForSeconds(0.1f);
            }

            dopaminBar.waiting = false;
            player.jetons += _lastResult;
            _gamblingAnim = false;
            _inputLocked = false;
            _isActive = false;
        }

        public IEnumerator FillSyringe() {
            dopaminBar.waiting = true;
            yield return new WaitForSeconds(1.3f);
            while (dopaminBar.time < 10) {
                player.time += 0.3f;
                dopaminBar.time += 0.3f;
                dopaminBar.slider.value += 0.3f;
                yield return new WaitForSeconds(0.05f);
                if (dopaminBar.time >= 10) break;
            }

            player.time = 10;
            dopaminBar.slider.value = 10;
            dopaminBar.time = 10;
        }
    }
}
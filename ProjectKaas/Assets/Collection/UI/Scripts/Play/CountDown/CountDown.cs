using System.Collections;
using System.Timers;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.Play.CountDown
{
    public class CountDown : MonoBehaviour
    {
        #region Singleton

        public static CountDown Instance;

        #endregion
        
        #region private Serializeable Fields

        [SerializeField] private TextMeshProUGUI guiTimer;
        [SerializeField] private TextMeshProUGUI startText;

        #endregion

        #region Private Fields

        private float _timer;
        private bool _startTimer;

        #endregion

        #region Pubic Events

        public delegate void TimerFinished();
        public TimerFinished OnTimerFinished;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void FixedUpdate()
        {
            if (_startTimer)
            {
                _timer -= Time.deltaTime;
                guiTimer.text = ((int) _timer).ToString();
                if (_timer < 0)
                {
                    OnTimerFinished?.Invoke();
                    _startTimer = false;
                }
            }
        }

        #endregion

        #region Public Methods

        public void StartTimer(float startTime)
        {
            _timer = startTime;
            guiTimer.text = ((int) _timer).ToString();
            guiTimer.gameObject.SetActive(true);
            startText.gameObject.SetActive(true);
            _startTimer = true;
        }

        public void CancelTimer()
        {
            _startTimer = false;
            guiTimer.gameObject.SetActive(false);
            startText.gameObject.SetActive(false);
        }

        #endregion
    }
}

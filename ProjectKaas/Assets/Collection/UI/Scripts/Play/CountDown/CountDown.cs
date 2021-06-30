using System.Collections;
using System.Globalization;
using System.Timers;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play.CountDown
{
    public class CountDown : MonoBehaviour
    {
        #region private Serializeable Fields

        [SerializeField] private Text guiTimer;
        [SerializeField] private CountdownTimer timer;

        #endregion

        #region Pubic Events

        public delegate void TimerFinished();
        public TimerFinished OnTimerFinished;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            timer.OnEnable();
            CountdownTimer.OnCountdownTimerHasExpired += TimerEvent;
            
            if (PhotonNetwork.IsMasterClient)
            {
                CountdownTimer.SetStartTime();
            }
        }

        private void Start()
        {
            timer.Start();
        }

        private void Update()
        {
            timer.Update();
        }

        private void OnDisable()
        {
            timer.OnDisable();
            CountdownTimer.OnCountdownTimerHasExpired -= TimerEvent;
        }

        #endregion

        #region Private Methods

        private void TimerEvent()
        {
            Debug.Log("Timer Expired");
            OnTimerFinished?.Invoke();
        }

        #endregion

        #region Public Methods

        public void StartTimer()
        {
            gameObject.SetActive(true);
        }

        public void CancelTimer()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}

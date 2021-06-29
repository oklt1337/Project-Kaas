using System.Collections.Generic;
using Collection.UI.Scripts;
using Collection.UI.Scripts.Play.CountDown;
using Collection.UI.Scripts.Play.Room;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace Collection.ReadyUpManager.Scripts
{
    public class ReadyUpManager : MonoBehaviour
    {
        #region Public Singleton

        public static ReadyUpManager Instance;

        #endregion

        #region Public Fields

        public int LobbySize { get; set; }
        
        private List<PlayerListing> ReadyPlayer { get; } = new List<PlayerListing>();

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Private Methods

        private void StartMapVeto()
        {
            OverlayCanvases.Instance.VoteMapCanvas.gameObject.SetActive(true);
        }

        #endregion

        #region Public Methods

        public void AddReadyPlayer(PlayerListing playerListing)
        {
            if (!ReadyPlayer.Contains(playerListing))
            {
                ReadyPlayer.Add(playerListing);
            }

            if (ReadyPlayer.Count == LobbySize)
            {
                Debug.Log("All Ready");
                CountDown.Instance.StartTimer(11f);

                if (PhotonNetwork.IsMasterClient)
                {
                    CountDown.Instance.OnTimerFinished += StartMapVeto;
                }
            }
        }
        
        public void RemoveReadyPlayer(PlayerListing playerListing)
        {
            if (ReadyPlayer.Contains(playerListing))
            {
                ReadyPlayer.Remove(playerListing);
            }

            if (ReadyPlayer.Count != LobbySize)
            {
                Debug.Log("Sb is not ready anymore.");
                
                if (PhotonNetwork.IsMasterClient)
                {
                    CountDown.Instance.OnTimerFinished -= StartMapVeto;
                }
                CountDown.Instance.CancelTimer();
            }
        }
        
        #endregion
    }
}

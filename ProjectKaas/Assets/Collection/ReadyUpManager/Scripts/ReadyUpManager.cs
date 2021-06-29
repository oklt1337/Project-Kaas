using System.Collections.Generic;
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

        private void StartGame()
        {
            Debug.Log("Load map...");

            // Load the Map.
            PhotonNetwork.LoadLevel("Map " + Random.Range(1, 3));
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
                    CountDown.Instance.OnTimerFinished += StartGame;
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
                    CountDown.Instance.OnTimerFinished -= StartGame;
                }
                CountDown.Instance.CancelTimer();
            }
        }
        
        #endregion
    }
}

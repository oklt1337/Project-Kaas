using System.Collections.Generic;
using Collection.UI.Scripts;
using Collection.UI.Scripts.Play.CountDown;
using Collection.UI.Scripts.Play.Room;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.ReadyUpManager.Scripts
{
    public class ReadyUpManager : MonoBehaviourPunCallbacks
    {
        #region Public Singleton

        public static ReadyUpManager Instance;

        #endregion

        [SerializeField] private CountDown countDown;

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
            
            ReadyPlayer.Clear();
        }

        #endregion

        #region Photon Callbacks

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey("StartMatch"))
            {
                if ((bool) propertiesThatChanged["StartMatch"])
                {
                    countDown.StartTimer();
                
                    if (PhotonNetwork.IsMasterClient)
                    {
                        countDown.OnTimerFinished += StartMapVeto;
                    }
                }
                else
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        countDown.OnTimerFinished -= StartMapVeto;
                    }
                    countDown.CancelTimer();
                }
            }
            
            if (propertiesThatChanged.ContainsKey("StartMapVeto"))
            {
                Debug.Log("Start Map Veto");
                
                if ((bool) propertiesThatChanged["StartMapVeto"])
                {
                    Debug.Log("StartMapVeto");
                    OverlayCanvases.Instance.VoteMapCanvas.gameObject.SetActive(true);
                }
            }
        }

        #endregion

        #region Private Methods

        private void StartMapVeto()
        {
            var properties = PhotonNetwork.CurrentRoom.CustomProperties;

            if (properties.ContainsKey("StartMapVeto"))
            {
                properties["StartMapVeto"] = true;
            }
            else
            {
                properties.Add("StartMapVeto", true);
            }

            if (properties.ContainsKey("StartMatch"))
            {
                properties["StartMatch"] = false;
            }
            
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            countDown.OnTimerFinished -= StartMapVeto;
        }

        #endregion

        #region Public Methods

        public void AddReadyPlayer(PlayerListing playerListing)
        {
            Debug.Log(ReadyPlayer.Count + " Players are ready");
            if (!ReadyPlayer.Contains(playerListing))
            {
                ReadyPlayer.Add(playerListing);
            }

            if (ReadyPlayer.Count == LobbySize)
            {
                Debug.Log("All Ready");

                var properties = PhotonNetwork.CurrentRoom.CustomProperties;

                if (properties.ContainsKey("StartMatch"))
                {
                    properties["StartMatch"] = true;
                }
                else
                {
                    properties.Add("StartMatch", true);
                }
                
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
        }
        
        public void RemoveReadyPlayer(PlayerListing playerListing)
        {
            Debug.Log(ReadyPlayer.Count + " Players are ready");
            
            if (ReadyPlayer.Contains(playerListing))
            {
                ReadyPlayer.Remove(playerListing);
            }

            if (ReadyPlayer.Count != LobbySize)
            {
                Debug.Log("Sb is not ready anymore.");
                
                var properties = PhotonNetwork.CurrentRoom.CustomProperties;
                if (properties.ContainsKey("StartMatch"))
                {
                    properties["StartMatch"] = false;
                }
                else
                {
                    properties.Add("StartMatch", false);
                }
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            }
        }
        
        #endregion
    }
}

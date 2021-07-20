using System.Collections;
using Collection.Authentication.Scripts;
using Collection.Profile.Scripts;
using Collection.UI.Scripts;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Collection.Network.Scripts
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Singleton

        public static NetworkManager Instance;

        #endregion

        #region Private Fields

        /// <summary>
        /// Coroutine for setting the ping.
        /// </summary>
        private Coroutine _pingCo;

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
            
            PlayFabAuthManager.OnLogOut.AddListener(SetRandomDefaultNickName);
            LocalProfile.OnProfileInitialized.AddListener(SetPhotonProfileValues);
        }

        #endregion

        #region PhotonNetwork Callbacks

        public override void OnConnectedToMaster()
        {
            // Make sure coroutine doesnt run twice.
            if (_pingCo != null)
                StopCoroutine(_pingCo);
            
            _pingCo = StartCoroutine(SetPingCo());
            
            var scene = SceneManager.GetActiveScene();
            if (!PhotonNetwork.InLobby && scene.buildIndex != 0)
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");

            TurnBackToRoom();
        }

        #endregion
        
        #region Private Methods

        private void TurnBackToRoom()
        {
            var hashtable = PhotonNetwork.LocalPlayer.CustomProperties;

            if (!hashtable.ContainsKey("MatchFinished")) return;
            if (!(bool) hashtable["MatchFinished"]) return;
            if (!hashtable.ContainsKey("WasMasterClient")) return;
            
            if ((bool) hashtable["WasMasterClient"])
            {
                if (!hashtable.ContainsKey("OldRoom")) return;
                        
                PhotonNetwork.CreateRoom((string) hashtable["OldRoom"]);

                OverlayCanvases.Instance.RoomListCanvas.gameObject.SetActive(true);
                OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(true);
                
                hashtable.Remove("MatchFinished");
                hashtable.Remove("WasMasterClient");
                hashtable.Remove("OldRoom");

                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            }
            else
            {
                PhotonNetwork.JoinRoom((string) hashtable["OldRoom"]);
                OverlayCanvases.Instance.RoomListCanvas.gameObject.SetActive(true);
                OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(true);

                hashtable.Remove("MatchFinished");
                hashtable.Remove("WasMasterClient");
                hashtable.Remove("OldRoom");
                
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            }
        }

        private static IEnumerator SetPingCo()
        {
            while (PhotonNetwork.IsConnected)
            {
                var hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
                if (!hashtable.ContainsKey("Ping"))
                {
                    hashtable.Add("Ping", PhotonNetwork.GetPing());
                }
                else
                {
                    hashtable["Ping"] = PhotonNetwork.GetPing();
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

                yield return new WaitForSeconds(5f);
            }
        }

        /// <summary>
        /// Set Photon Nickname to PlayFab displayName.
        /// Set authValue to PlayFab playerId
        /// </summary>
        private static void SetPhotonProfileValues()
        {
            PhotonNetwork.LocalPlayer.NickName = LocalProfile.Instance.PlayerProfileModel.DisplayName;
            PhotonNetwork.AuthValues.UserId = LocalProfile.Instance.PlayerProfileModel.PlayerId;
        }

        #endregion

        #region Public Methods

        public static void SetRandomDefaultNickName()
        {
            PhotonNetwork.LocalPlayer.NickName = "MusterName#" + Random.Range(1000, 9999);
        }

        #endregion
    }
}

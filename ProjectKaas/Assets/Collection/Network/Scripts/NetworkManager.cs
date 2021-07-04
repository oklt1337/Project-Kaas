using System.Collections;
using Collection.Authentication.Scripts;
using Collection.Profile.Scripts;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        /// <summary>
        /// Coroutine for setting the ping.
        /// </summary>
        private Coroutine _pingCo;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
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
        }

        #endregion
        
        #region Private Methods

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
            var authenticationValues = new AuthenticationValues(LocalProfile.Instance.PlayerProfileModel.PlayerId);
            PhotonNetwork.AuthValues = authenticationValues;
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

using System.Collections;
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Collection.Network.Scripts
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        /// <summary>
        /// Custom Properties for LocalPlayer
        /// </summary>
        private Hashtable _playerProperties = new Hashtable();
        
        /// <summary>
        /// Coroutine for setting the ping.
        /// </summary>
        private Coroutine _pingCo;

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

        private IEnumerator SetPingCo()
        {
            while (PhotonNetwork.IsConnected)
            {
                _playerProperties["Ping"] = PhotonNetwork.GetPing();
                PhotonNetwork.LocalPlayer.SetCustomProperties(_playerProperties);

                yield return new WaitForSeconds(5f);
            }
        }

        #endregion
    }
}

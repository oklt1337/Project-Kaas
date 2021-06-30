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

        #endregion
    }
}

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Collection.GameManager.Scripts
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Photon Callbacks
        
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        
        public override void OnPlayerEnteredRoom(Player other)
        {
            // not seen if you're the player connecting
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); 
            
            if (PhotonNetwork.IsMasterClient)
            {
                // called before OnPlayerLeftRoom
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 
                
                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            // seen when other disconnects
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); 

            if (PhotonNetwork.IsMasterClient)
            {
                // called before OnPlayerLeftRoom
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); 
                
                LoadArena();
            }
        }
        
        #endregion
        
        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
        
        #region Private Methods

        private void LoadArena()
        {
            // Make sure only MasterClient can LoadLevel
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork: Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork: Loading Level: {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Map " + Random.Range(1,3));
        }

        #endregion
    }
}

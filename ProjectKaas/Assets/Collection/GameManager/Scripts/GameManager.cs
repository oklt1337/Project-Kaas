using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Collection.GameManager.Scripts
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        #endregion

        #region Private SerializeFields 

        [SerializeField] private Transform[] startPos;
        [SerializeField] private GameObject pauseMenu;

        #endregion

        #region Monobehaviour Callbacks

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                
                Debug.Log("Instantiating LocalPlayer.");

                // Get random Start pos (need just to test)
                var pos = Random.Range(0, startPos.Length);
                
                // Spawn playerPrefab for the local player.
                PhotonNetwork.Instantiate("Prefabs/" + playerPrefab.name, startPos[pos].position, Quaternion.identity);
            }
        }

        #endregion
        
        #region Photon Callbacks
        
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(1);
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


        public void Continue()
        {
            pauseMenu.SetActive(false);
        }
        
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void Quit()
        {
            LeaveRoom();
            Application.Quit();
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

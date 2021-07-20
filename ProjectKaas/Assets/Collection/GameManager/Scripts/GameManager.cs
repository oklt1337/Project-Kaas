using System;
using System.Collections.Generic;
using System.Linq;
using Collection.Items.Scripts;
using Collection.Maps.Scripts;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Collection.GameManager.Scripts
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public static GameManager Gm;

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        public ItemBehaviour[] AllItems => allItems;

        public readonly List<Player> Players = new List<Player>();

        #endregion

        #region Private SerializeFields

        [SerializeField] private Transform[] startPos;
        [SerializeField] private ItemBehaviour[] allItems;
        [SerializeField] private GameObject pauseMenu;

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            Gm = this;
        }

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError(
                    "<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",
                    this);
            }
            else
            {
                Debug.Log("Instantiating LocalPlayer.");

                var index = 0;
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Matchmaking"))
                {
                    if ((bool) PhotonNetwork.CurrentRoom.CustomProperties["Matchmaking"])
                    {
                        index = Random.Range(0, startPos.Length);
                    }
                }
                else
                {
                    // Get random Start pos (need just to test)
                    index = (int) PhotonNetwork.LocalPlayer.CustomProperties["Position"];
                }

                // Spawn playerPrefab for the local player.
                PhotonNetwork.Instantiate("Prefabs/Player", startPos[index].position, Quaternion.identity);
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
            }
        }

        #endregion

        #region MyRegion

        [PunRPC]
        private void RPCLeaveMatch()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Public Methods

        public void AddPlayer(PlayerHandler playerHandler)
        {
            Players.Add(playerHandler);
        }

        public void Continue()
        {
            pauseMenu.SetActive(false);
        }

        public void LeaveRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPCLeaveMatch", RpcTarget.All);
            }
            else
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene(1);
            }
        }

        public void Quit()
        {
            LeaveRoom();
            Application.Quit();
        }

        public void OnMatchFinished()
        {
            var hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
            
            if (hashtable.ContainsKey("MatchFinished"))
                hashtable["MatchFinished"] = true;
            else
                hashtable.Add("MatchFinished", true);

            if (hashtable.ContainsKey("OldRoom"))
                hashtable["OldRoom"] = PhotonNetwork.CurrentRoom.Name;
            else
                hashtable.Add("OldRoom", PhotonNetwork.CurrentRoom.Name);

            if (hashtable.ContainsKey("WasMasterClient"))
                hashtable["WasMasterClient"] = PhotonNetwork.IsMasterClient;
            else
                hashtable.Add("WasMasterClient", PhotonNetwork.IsMasterClient);
        }

        #endregion
    }
}

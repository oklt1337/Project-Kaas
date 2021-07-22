using System;
using System.Collections.Generic;
using System.Linq;
using Collection.Items.Scripts;
using Collection.Maps.Scripts;
using Collection.NetworkPlayer.Scripts;
using Collection.UI.Scripts.Play.ChoosingCar;
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

        public List<PlayerHandler> PlayerHandlers => playerHandler;

        #endregion

        #region Private SerializeFields

        [SerializeField] private Transform[] startPos;
        [SerializeField] private ItemBehaviour[] allItems;
        [SerializeField] private List<PlayerHandler> playerHandler;
        [SerializeField] private bool reSkinDone;

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

                // Get random Start pos (need just to test)
                var index = (int) PhotonNetwork.LocalPlayer.CustomProperties["Position"];

                // Spawn playerPrefab for the local player.
                PhotonNetwork.Instantiate("Prefabs/Player", startPos[index].position, Quaternion.identity);
            }

            
        }

        private void LateUpdate()
        {
            //pls dont look at this it had to be done quickly.
            if (reSkinDone || playerHandler.Count != PhotonNetwork.CurrentRoom.Players.Count) return;
            ReSkinCars();
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

        #region RPC

        [PunRPC]
        private void RPCLeaveMatch()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Public Methods

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

            if (hashtable.ContainsKey("MaxPlayer"))
                hashtable["MaxPlayer"] = PhotonNetwork.CurrentRoom.MaxPlayers;
            else
                hashtable.Add("MaxPlayer", PhotonNetwork.CurrentRoom.MaxPlayers);
        }

        #endregion

        #region Private Methods

        private void ReSkinCars()
        {
            var players = PhotonNetwork.CurrentRoom.Players.Values.ToList();
            foreach (var player in players)
            {
                if (!Equals(player, PhotonNetwork.LocalPlayer))
                {
                    var hashtable = player.CustomProperties;
                    var index = playerHandler.FindIndex(x => x.ActorNumber == player.ActorNumber);

                    if (index != -1)
                    {
                        var handler = this.playerHandler[index];

                        if (hashtable.ContainsKey("Car"))
                        {
                            var chooseCar = (ChooseCar) hashtable["Car"];
                            handler.ReInit(chooseCar);
                            reSkinDone = true;
                        }
                    }
                }
            }
        }

        #endregion
    }
}

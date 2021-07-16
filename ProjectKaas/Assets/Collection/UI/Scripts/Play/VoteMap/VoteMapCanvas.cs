using System;
using System.Collections.Generic;
using Collection.Network.Scripts;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Collection.UI.Scripts.Play.VoteMap
{
    public class VoteMapCanvas : MonoBehaviourPunCallbacks
    {
        #region Const

        private const string City = "City";
        private const string Japan = "Japan";
        private const string Mountains = "Mountains";

        #endregion

        #region Private Serializable Fields

        [SerializeField] private TextMeshProUGUI timer;

        #endregion

        #region Private Fields

        private float _timer;
        private bool _timerStart;
        private int _numberOfVoters;


        private readonly List<Player> _didVote = new List<Player>();
        private readonly Dictionary<string, int> _mapVeto = new Dictionary<string, int>
        {
            [City] = 0,
            [Japan] = 0,
            [Mountains] = 0
        };

        #endregion

        #region MonoBehaviour Callbacks

        private void FixedUpdate()
        {
            if (_timerStart)
            {
                _timer -= Time.fixedDeltaTime;
                timer.text = ((int) _timer).ToString();
                if (_timer < 0)
                {
                    ForceMapVetoEnd();
                }
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnEnable()
        {
            base.OnEnable();
            OverlayCanvases.Instance.PlayerInfoCanvas.gameObject.SetActive(false);
            OverlayCanvases.Instance.CurrenRoomCanvas.SetTouchableState(false);
            
            _timerStart = true;
            _timer = 60;
            timer.text = ((int) _timer).ToString();
        }

        #endregion

        #region Private Methods

        private void ForceMapVetoEnd()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var mapWon = String.Empty;

            // check witch map got the most votes
            if (_mapVeto[City] > _mapVeto[Japan] && _mapVeto[City] > _mapVeto[Mountains])
            {
                mapWon = City;
            }
            else if (_mapVeto[Japan] > _mapVeto[City] && _mapVeto[Japan] > _mapVeto[Mountains])
            {
                mapWon = Japan;
            }
            else if (_mapVeto[Mountains] > _mapVeto[City] && _mapVeto[Mountains] > _mapVeto[Japan])
            {
                mapWon = Mountains;
            }

            // if no one votes generate random map.
            if (mapWon == String.Empty)
            {
                var randomMap = Random.Range(0, 3);
                mapWon = randomMap switch
                {
                    0 => City,
                    1 => Japan,
                    2 => Mountains,
                    _ => City
                };
            }

            // Load the Map.
            PhotonNetwork.LoadLevel(mapWon);
            Debug.Log($"Loading {mapWon}");
        }

        private void VoteMap(string map, Player player)
        {
            if (!_didVote.Contains(player))
            {
                _mapVeto[map]++;
                _numberOfVoters++;
                _didVote.Add(player);
            }

            if (_numberOfVoters == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                ForceMapVetoEnd();
            }
        }

        [PunRPC]
        private void RPCVetoMap(string map, Player player)
        {
            VoteMap(map, player);
        }

        #region Public Methods

        public void OnClickMap1()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, City, PhotonNetwork.LocalPlayer);
        }

        public void OnClickMap2()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Japan, PhotonNetwork.LocalPlayer);
        }

        public void OnClickMap3()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Mountains, PhotonNetwork.LocalPlayer);
        }

        #endregion

        #endregion
    }
}

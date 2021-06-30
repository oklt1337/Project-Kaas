using System;
using System.Collections.Generic;
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

        private const string Map1 = "Map 1";
        private const string Map2 = "Map 2";
        private const string Map3 = "Map 3";

        #endregion

        #region Private Serializable Fields

        [SerializeField] private TextMeshProUGUI timer;

        #endregion

        #region Private Fields

        private float _timer;
        private bool _timerStart;
        private int _numberOfVoters;


        private List<Player> _didVote = new List<Player>();
        private readonly Dictionary<string, int> _mapVeto = new Dictionary<string, int>
        {
            [Map1] = 0,
            [Map2] = 0,
            [Map3] = 0
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
            if (_mapVeto[Map1] > _mapVeto[Map2] && _mapVeto[Map1] > _mapVeto[Map3])
            {
                mapWon = Map1;
            }
            else if (_mapVeto[Map2] > _mapVeto[Map1] && _mapVeto[Map2] > _mapVeto[Map3])
            {
                mapWon = Map2;
            }
            else if (_mapVeto[Map3] > _mapVeto[Map1] && _mapVeto[Map3] > _mapVeto[Map2])
            {
                mapWon = Map3;
            }

            // if no one votes generate random map.
            if (mapWon == String.Empty)
            {
                var randomMap = Random.Range(0, 3);
                mapWon = randomMap switch
                {
                    0 => Map1,
                    1 => Map2,
                    2 => Map3,
                    _ => Map1
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
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Map1, PhotonNetwork.LocalPlayer);
        }

        public void OnClickMap2()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Map2, PhotonNetwork.LocalPlayer);
        }

        public void OnClickMap3()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Map3, PhotonNetwork.LocalPlayer);
        }

        #endregion

        #endregion
    }
}

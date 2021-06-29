using System;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

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
            var mapWon = string.Empty;

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
            
            // Load the Map.
            PhotonNetwork.LoadLevel(mapWon);
            Debug.Log($"Loading {mapWon}");
        }
        
        private void VoteMap(string map)
        {
            _mapVeto[map]++;
            _numberOfVoters++;

            Debug.Log(_numberOfVoters);
            Debug.Log(PhotonNetwork.CurrentRoom.MaxPlayers);
            
            if (_numberOfVoters == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                ForceMapVetoEnd();
            }
        }
        
        [PunRPC]
        private void RPCVetoMap(string map)
        {
            VoteMap(map);
        }
        
        #region Public Methods

        public void OnClickMap1()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Map1);
        }
        
        public void OnClickMap2()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Map2);
        }
        
        public void OnClickMap3()
        {
            photonView.RPC("RPCVetoMap", RpcTarget.MasterClient, Map3);
        }

        #endregion

        #endregion
        
    }
}

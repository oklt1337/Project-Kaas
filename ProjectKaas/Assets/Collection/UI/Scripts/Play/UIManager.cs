using System;
using System.Collections.Generic;
using Collection.Cars.Scripts;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;
using static Collection.Maps.Scripts.PositionManager;

namespace Collection.UI.Scripts.Play
{
    public class UIManager : MonoBehaviourPun
    {
        public static UIManager UIManagerInstance;
        
        private Car _client;

        [SerializeField] private TextMeshProUGUI lapCount;
        [SerializeField] private TextMeshProUGUI position;

        private void Awake()
        {
            if (UIManagerInstance != null)
                Destroy(this);
            
            UIManagerInstance = this;
        }

        private void Update()
        {
            if(_client == null)
                return;
            
            UpdateInfo();
        }

        /// <summary>
        /// Updates the Info of the UI.
        /// </summary>
        private void UpdateInfo()
        {
            lapCount.text = _client.LapCount + "/" + PositionManagerInstance.LapCount;
            position.text = _client.place + "/" + PositionManagerInstance.AllPlayers.Count;
        }

        /// <summary>
        /// Finds the local player.
        /// </summary>
        /// <param name="players"> The players. </param>
        public void FindLocalPlayer(List<PlayerHandler> players)
        {
            for (var i = 0; i < players.Count; i++)
            {
                if (!Equals(players[i].LocalPlayer, PhotonNetwork.LocalPlayer)) 
                    continue;
                
                _client = players[i].Car;
                break;
            }
        }
    }
}

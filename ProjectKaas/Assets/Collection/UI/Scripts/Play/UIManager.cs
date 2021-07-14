using System;
using System.Collections.Generic;
using Collection.Cars.Scripts;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Collection.Maps.Scripts.PositionManager;

namespace Collection.UI.Scripts.Play
{
    public class UIManager : MonoBehaviourPun
    {
        public static UIManager UIManagerInstance;
        
        private PlayerHandler _client;
        
        [SerializeField] private TextMeshProUGUI lapCount;
        [SerializeField] private TextMeshProUGUI position;
        [SerializeField] private Image currentItem;

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
            lapCount.text = _client.Car.LapCount + "/" + PositionManagerInstance.LapCount;
            position.text = _client.Car.place + "/" + PositionManagerInstance.AllPlayers.Count;
            currentItem.sprite = _client.Item.itemSprite;
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
                
                _client = players[i];
                break;
            }
        }
    }
}

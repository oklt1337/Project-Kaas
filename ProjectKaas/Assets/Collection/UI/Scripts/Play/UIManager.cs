using System;
using System.Collections.Generic;
using System.Linq;
using Collection.Cars.Scripts;
using Collection.NetworkPlayer.Scripts;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Collection.Maps.Scripts.PositionManager;

namespace Collection.UI.Scripts.Play
{
    public class UIManager : MonoBehaviourPun
    {
        private PlayerHandler _client;

        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private TextMeshProUGUI lapCount;
        [SerializeField] private TextMeshProUGUI position;
        [SerializeField] private Image currentItem;

        private void Start()
        {
            ToggleUI();
            FindLocalPlayer(PositionManagerInstance.AllPlayers);
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
            position.text = (_client.Car.place+1) + ".";

            if (_client.Item != null)
            {
                currentItem.gameObject.SetActive(true);
                currentItem.sprite = _client.Item.itemSprite;
            }
            else
            {
                currentItem.gameObject.SetActive(false);
            }
        }

        public void OpenPauseMenu()
        {
            pauseScreen.SetActive(true);
        }

        public void Continue()
        {
            pauseScreen.SetActive(false);
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

        #region private Functions

        /// <summary>
        /// /// Finds the local player.
        /// </summary>
        /// <param name="players"> The players. </param>
        private void FindLocalPlayer(List<PlayerHandler> players)
        { 
            foreach (var t in players.Where(t => Equals(t.LocalPlayer, PhotonNetwork.LocalPlayer))) 
            { 
                _client = t; 
                break;
            }
        }
        
        /// <summary>
        /// /// Toggles the drive UI.
        /// </summary>
        private void ToggleUI()
        { 
            lapCount.gameObject.SetActive(!lapCount.gameObject.activeSelf); 
            position.gameObject.SetActive(!position.gameObject.activeSelf); 
            currentItem.gameObject.SetActive(!currentItem.gameObject.activeSelf);
        }

        #endregion
        
        
    }
}

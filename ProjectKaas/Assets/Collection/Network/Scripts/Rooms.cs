using System;
using Collection.UI.Scripts;
using Collection.UI.Scripts.Play;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.Network.Scripts
{
    public class Rooms : MonoBehaviourPunCallbacks
    {
        #region Private Sertializable Fields

        [SerializeField] private TMP_InputField roomName;
        [SerializeField] private TextMeshProUGUI maxPlayer;
        [SerializeField] private Toggle @public;
        [SerializeField] private TextMeshProUGUI warning;

        #endregion
        
        #region Private Fields

        private byte _maxPlayer = 1;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            warning.gameObject.SetActive(false);
            maxPlayer.text = _maxPlayer.ToString();
            roomName.text = String.Empty;
        }

        #endregion

        #region Public Methods

        public void OnClickAddOne()
        {
            _maxPlayer += 1;

            if (_maxPlayer > 8)
            {
                _maxPlayer = 8;
            }

            if (_maxPlayer < 1)
            {
                _maxPlayer = 1;
            }

            maxPlayer.text = _maxPlayer.ToString();
        }
        
        public void OnClickRemoveOne()
        {
            _maxPlayer -= 1;

            if (_maxPlayer > 8)
            {
                _maxPlayer = 8;
            }

            if (_maxPlayer < 1)
            {
                _maxPlayer = 1;
            }
            
            maxPlayer.text = _maxPlayer.ToString();
        }

        public void OnClickCreateRoom()
        {
            if (string.IsNullOrEmpty(roomName.text))
            {
                warning.gameObject.SetActive(true);
                warning.text = "Warning: Room name cant be empty.";
            }
            else
            {
                var roomOptions = new RoomOptions()
                {
                    IsVisible = @public.isOn,
                    IsOpen = @public.isOn,
                    MaxPlayers = _maxPlayer
                };
            
                PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);

                OverlayCanvases.Instance.CurrenRoomCanvas.SetActive(true);
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            
            warning.text = String.Empty;
            warning.gameObject.SetActive(false);
            roomName.text = String.Empty;
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left room.");
            warning.text = String.Empty;
            warning.gameObject.SetActive(false);
            roomName.text = String.Empty;
        }

        #endregion
    }
}

using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play.CreateRoom
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
        
        #region Photon Callbacks

        public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(true);
            
            warning.text = String.Empty;
            warning.gameObject.SetActive(false);
            roomName.text = String.Empty;
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left room.");
            
            OverlayCanvases.Instance.CurrenRoomCanvas.gameObject.SetActive(false);
            OverlayCanvases.Instance.PlayerInfoCanvas.gameObject.SetActive(false);

            StartCoroutine(WarningCo("You left the room or got kicked."));
            
            var customProp = PhotonNetwork.LocalPlayer.CustomProperties;
            if (customProp.ContainsKey("Room"))
            {
                customProp["Room"] = String.Empty;
            }
            else
            {
                customProp.Add("Room", String.Empty);
            }

            PhotonNetwork.LocalPlayer.SetCustomProperties(customProp);
            
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
                var roomOptions = new RoomOptions
                {
                    IsVisible = @public.isOn, 
                    IsOpen = @public.isOn, 
                    MaxPlayers = _maxPlayer, 
                    PublishUserId = true
                };
                
                PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
                ReadyUpManager.Scripts.ReadyUpManager.Instance.LobbySize = _maxPlayer;
            }
        }

        public void OnClickBack()
        {
            OverlayCanvases.Instance.RoomListCanvas.gameObject.SetActive(false);
        }

        #endregion
        
        #region Private Methods

        private IEnumerator WarningCo(string text)
        {
            warning.gameObject.SetActive(true);
            warning.text = text;

            yield return new WaitForSeconds(5f);

            warning.text = String.Empty;
            warning.gameObject.SetActive(false);
        }
        
        #endregion
    }
}

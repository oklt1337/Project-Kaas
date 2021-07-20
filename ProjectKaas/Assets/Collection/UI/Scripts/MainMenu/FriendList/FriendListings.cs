using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FriendInfo = Photon.Realtime.FriendInfo;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendListings : MonoBehaviourPunCallbacks
    {
        #region Private Serialisable Fields

        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private RawImage onlineStatus;

        #endregion

        #region Public Fields

        public PlayFab.ClientModels.FriendInfo Friend { get; set; }
        public FriendInfo PhotonFriend { get; set; }
        public RawImage OnlineStatus => onlineStatus;
        public TextMeshProUGUI DisplayName => displayName;

        #endregion

        #region Private Fields

        private float _buffer = 60f;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClickFriendInfo);
        }

        private void Update()
        {
            if (_buffer <= 0)
            {
                if (PhotonFriend != null)
                {
                    SetOnlineStatus(PhotonFriend.IsOnline, PhotonFriend.IsInRoom);
                }
                else
                {
                    SetOnlineStatus(false, false);
                }

                _buffer = 60f;
            }
            else
            {
                _buffer -= Time.deltaTime;
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            base.OnFriendListUpdate(friendList);

            var index = friendList.FindIndex(x => x.UserId == Friend.FriendPlayFabId);

            if (index != -1)
            {
                PhotonFriend = friendList[index];
                Debug.Log("Photon FriendList updated.");
                SetOnlineStatus(PhotonFriend.IsOnline, PhotonFriend.IsInRoom);
            }
            else
            {
                Debug.Log("Friend is not in list.");
            }
        }

        #endregion

        #region Public Methods

        public void ApplyFriend(PlayFab.ClientModels.FriendInfo friend)
        {
            Friend = friend;
            DisplayName.text = friend.TitleDisplayName;
            
            SetOnlineStatus(false,false);
            
            var friendArray = new string[1];
            friendArray[0] = friend.FriendPlayFabId;

            PhotonNetwork.FindFriends(friendArray);
        }

        #endregion
        
        #region Private Methods

        private void SetOnlineStatus(bool online, bool inRoom)
        {
            if (PhotonFriend != null)
            {
                Debug.Log(PhotonFriend.UserId + " is online = " + online);
                Debug.Log(PhotonFriend.UserId + " is in room = " + inRoom);
            }

            if (online && !inRoom)
            {
                OnlineStatus.color = Color.green;
            }
            else if (inRoom && !online)
            {
                OnlineStatus.color = Color.yellow;
            }
            else
            {
                OnlineStatus.color = Color.red;
            }
        }
        
        private void OnClickFriendInfo()
        {
            Debug.Log("Friend info of: " + displayName.text);
            
            MainMenuCanvases.Instance.FriendInfoCanvas.gameObject.SetActive(true);
            MainMenuCanvases.Instance.FriendInfoCanvas.Initialize(Friend);
        }

        #endregion
    }
}

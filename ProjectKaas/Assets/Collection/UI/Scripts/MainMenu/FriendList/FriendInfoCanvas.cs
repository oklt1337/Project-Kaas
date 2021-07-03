using System;
using System.Linq;
using Collection.FriendList.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendInfoCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Button profile;
        [SerializeField] private Button chat;
        [SerializeField] private Button join;
        [SerializeField] private Button remove;

        #endregion

        #region Private Fields

        private PlayFab.ClientModels.FriendInfo _friend;

        #endregion

        #region Public Methods

        public void Initialize(PlayFab.ClientModels.FriendInfo friend)
        {
            Debug.Log("Init Player Info");
            _friend = friend;
            profile.onClick.AddListener(OnClickProfile);
            chat.onClick.AddListener(OnClickChat);
            join.onClick.AddListener(OnClickJoin);
            remove.onClick.AddListener(OnClickRemove);
        }

        public void OnClickBack()
        {
            _friend = null;
            gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        private void OnClickProfile()
        {
            // open profile

            _friend = null;
            gameObject.SetActive(false);
        }

        private void OnClickChat()
        {
            //open chat

            _friend = null;
            gameObject.SetActive(false);
        }

        private void OnClickJoin()
        {
            var index = FriendListLayoutGroup.FriendList.FindIndex(x => x.Friend == _friend);

            if (index != -1)
            {
                if (FriendListLayoutGroup.FriendList[index].PhotonFriend != null)
                {
                    var room = FriendListLayoutGroup.FriendList[index].PhotonFriend.Room;
                
                    if (!string.IsNullOrEmpty(room))
                    {
                        PhotonNetwork.JoinRoom(room);
                    }
                }
            }
            
            _friend = null;
            gameObject.SetActive(false);
        }

        private void OnClickRemove()
        {
            FriendRequester.DeleteFriend(_friend.FriendPlayFabId);

            _friend = null;
            gameObject.SetActive(false);
        }

        #endregion
    }
}

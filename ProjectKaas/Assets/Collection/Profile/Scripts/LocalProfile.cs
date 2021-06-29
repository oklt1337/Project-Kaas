using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Collection.Profile.Scripts
{
    public class LocalProfile : MonoBehaviour
    {

        #region Private Fields

        private List<FriendInfo> _friendList = new List<FriendInfo>();

        #endregion
        
        #region Public Fields

        public PlayerProfileModel PlayerProfileModel { get; private set; } = new PlayerProfileModel();

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), 
                result =>
                {
                    PlayerProfileModel = result.PlayerProfile;
                }, 
                error =>
                {
                    Debug.LogError($"Profile Not Found: {error.ErrorMessage}");
                });
            
            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(), 
                result =>
                {
                    _friendList = result.Friends;
                }, 
                error =>
            {
                Debug.LogError($"FriendList not found: {error.ErrorMessage}");
            });
        }

        #endregion
    }
}

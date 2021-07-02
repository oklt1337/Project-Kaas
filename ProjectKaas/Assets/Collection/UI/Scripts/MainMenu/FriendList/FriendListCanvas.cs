using System;
using System.Collections.Generic;
using Collection.FriendList.Scripts;
using Collection.Profile.Scripts;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendListCanvas : MonoBehaviourPunCallbacks
    {
        #region Private Serializeable Fields

        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TextMeshProUGUI playerID;

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            LocalProfile.OnProfileInitialized.AddListener(SetIDText);
        }

        #endregion

        #region Photon Callbacks

        public override void OnEnable()
        {
            base.OnEnable();
            //SetIDText(LocalProfile.Instance.PlayerProfileModel);
        }

        #endregion

        #region Public Methods

        public void OnClickAdd()
        {
            if (usernameInputField.text == String.Empty)
                return;

            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
                {
                    PlayFabId = usernameInputField.text
                },
                result => { FriendRequester.Instance.SendFriendRequest(result.PlayerProfile); },
                    error => { Debug.LogError($"Get Profile Error: {error.ErrorMessage}"); });
        }

        #endregion

        #region Private Methods

        private void SetIDText(PlayerProfileModel profileModel)
        {
            Debug.Log("Set ID text");
            playerID.text = profileModel.PlayerId;
        }

        #endregion
    }
}

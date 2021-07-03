using System;
using System.Collections;
using Collection.FriendList.Scripts;
using Photon.Pun;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendListCanvas : MonoBehaviour
    {
        #region Private Serializeable Fields

        [SerializeField] private FriendListLayoutGroup friendListLayoutGroup;
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TextMeshProUGUI playerID;
        [SerializeField] private GameObject copyText;

        #endregion

        #region Private Fields

        private string _copiedText;

        #endregion

        #region Public Fields

        public FriendListLayoutGroup FriendListLayoutGroup => friendListLayoutGroup;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnDisable()
        {
            if (MainMenuCanvases.Instance.FriendInfoCanvas.gameObject.activeSelf)
            {
                MainMenuCanvases.Instance.FriendInfoCanvas.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Public Methods

        public void OnClickAdd()
        {
            if (usernameInputField.text == String.Empty)
                return;
            
            FriendRequester.AddFriend(usernameInputField.text);
        }

        public void SetIDText(PlayerProfileModel profileModel)
        {
            playerID.text = profileModel.PlayerId;
        }

        public void OnClickCopyID()
        {
            _copiedText = playerID.text;
            StartCoroutine(CopyCo());
        }

        #endregion

        #region Private Methods

        private IEnumerator CopyCo()
        {
            copyText.SetActive(true);
            yield return new WaitForSeconds(1f);
            copyText.SetActive(false);
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Linq;
using Collection.FriendList.Scripts;
using Collection.Profile.Scripts;
using Photon.Pun;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendRequestCanvas : MonoBehaviourPun
    {
        #region Private Serializabel Fields

        [SerializeField] private TextMeshProUGUI requestText;
        [SerializeField] private Button accept;
        [SerializeField] private Button decline;

        #endregion

        #region Private Fields

        private const string SendText = " send you a friend request.";
        private string _requesterID;

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            FriendRequester.OnFriendRequestDeclined.AddListener(OnFriendRequestDeclined);
        }

        #endregion

        #region Public Methods

        public void Initialize(PlayerProfileModel requester)
        {
            requestText.text = requester.DisplayName + SendText;
            _requesterID = requester.PlayerId;

            accept.onClick.AddListener(OnClickAccept);
            decline.onClick.AddListener(OnClickDecline);
        }

        #endregion

        #region Private Methods

        private void OnClickAccept()
        {
            FriendRequester.OnFriendRequestAccepted?.Invoke(_requesterID);

            // find requester user.
            var index = PhotonNetwork.PlayerList.ToList().FindIndex(x => x.UserId == _requesterID);

            // pars user to rpc.
            photonView.RPC("AcceptFriendRPC", PhotonNetwork.PlayerList[index].Get(int.Parse(_requesterID)),
                LocalProfile.Instance.PlayerProfileModel.PlayerId);

            ResetAll();
        }

        private void OnClickDecline()
        {
            // find requester user.
            var index = PhotonNetwork.PlayerList.ToList().FindIndex(x => x.UserId == _requesterID);
            // pars user to rpc.
            photonView.RPC("DeclineFriendRPC", PhotonNetwork.PlayerList[index].Get(int.Parse(_requesterID)),
                LocalProfile.Instance.PlayerProfileModel.PlayerId);

            ResetAll();
        }

        private void ResetAll()
        {
            // reset button.
            decline.onClick.RemoveAllListeners();
            accept.onClick.RemoveAllListeners();
            requestText.text = string.Empty;
            _requesterID = string.Empty;

            gameObject.SetActive(false);
        }

        private void OnFriendRequestDeclined(string friendID)
        {
            StartCoroutine(DeclinedCo(friendID));
        }

        private IEnumerator DeclinedCo(string friendID)
        {
            MainMenuCanvases.Instance.MainMenu.InfoText.gameObject.SetActive(true);
            MainMenuCanvases.Instance.MainMenu.InfoText.text = $"{friendID} declined yot friend request.";

            yield return new WaitForSeconds(3f);

            MainMenuCanvases.Instance.MainMenu.InfoText.gameObject.SetActive(false);
            MainMenuCanvases.Instance.MainMenu.InfoText.text = String.Empty;
        }

        #endregion

        #region Photon RPC

        [PunRPC]
        private void AcceptFriendRPC(string myID)
        {
            FriendRequester.OnFriendRequestAccepted?.Invoke(myID);
        }

        [PunRPC]
        private void DeclineFriendRPC(string myID)
        {
            FriendRequester.OnFriendRequestDeclined?.Invoke(myID);
        }

        #endregion
    }
}

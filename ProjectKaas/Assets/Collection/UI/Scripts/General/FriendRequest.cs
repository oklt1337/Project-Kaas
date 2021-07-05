using Collection.FriendList.Scripts;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.General
{
    public class FriendRequest : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button declineButton;

        #endregion

        #region Private Fields

        private FriendInfo _friendInfo;

        #endregion

        #region Private Methods

        private void DeclineRequest()
        {
            FriendRequester.DenyFriendRequest(_friendInfo.FriendPlayFabId);
            Destroy(gameObject);
        }

        private void AcceptRequest()
        {
            FriendRequester.AcceptFriendRequest(_friendInfo.FriendPlayFabId);
            Destroy(gameObject);
        }

        #endregion

        #region Public Methods

        public void Initialize(FriendInfo friendInfo)
        {
            _friendInfo = friendInfo;
            displayName.text = _friendInfo.TitleDisplayName;
            
            acceptButton.onClick.AddListener(AcceptRequest);
            declineButton.onClick.AddListener(DeclineRequest);
        }

        #endregion
    }
}

using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendListings : MonoBehaviour
    {
        #region Private Serialisable Fields

        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private RawImage onlineStatus;

        #endregion

        #region Public Fields

        public FriendInfo Friend { get; set; }
        public RawImage OnlineStatus => onlineStatus;
        public TextMeshProUGUI DisplayName => displayName;

        #endregion

        #region Private Fields

        private float _buffer = 5f;

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
                SetOnlineStatus(Friend.IsOnline, Friend.IsInRoom);
                _buffer = 5f;
            }
            else
            {
                _buffer -= Time.deltaTime;
            }
        }

        #endregion

        #region Public Methods

        public void ApplyFriend(FriendInfo friend)
        {
            Friend = friend;
            DisplayName.text = friend.UserId;
            SetOnlineStatus(friend.IsOnline, friend.IsInRoom);
        }

        #endregion
        
        #region Private Methods

        private void SetOnlineStatus(bool online, bool isInRoom)
        {
            if (online && !isInRoom)
            {
                OnlineStatus.color = Color.green;
            }
            else if (isInRoom)
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

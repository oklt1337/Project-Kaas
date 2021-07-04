using Collection.FriendList.Scripts;
using Collection.Profile.Scripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play.Room
{
    public class PlayerInfoCanvas : MonoBehaviour
    {
        #region Private Serializable Fields

        [SerializeField] private Button add;
        [SerializeField] private Button kick;

        #endregion

        #region Private Fields

        private Player _player;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            kick.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }

        #endregion

        #region Public Methods

        public void Initialize(Player player)
        {
            Debug.Log("Init Player Info");
            _player = player;
            kick.onClick.AddListener(OnClickKick);
            add.onClick.AddListener(OnClickAdd);
        }

        public void OnClickBack()
        {
            _player = null;
            gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        private void OnClickAdd()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (PhotonNetwork.CurrentRoom.Players.ContainsValue(_player))
            {
                var index = LocalProfile.Instance.friendList.FindIndex(x => x.FriendPlayFabId == _player.UserId);
                if (index == -1)
                {
                    FriendRequester.SendFriendRequest(_player.UserId);
                }
            }
            
            _player = null;
            gameObject.SetActive(false);
        }

        private void OnClickKick()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            if (PhotonNetwork.CurrentRoom.Players.ContainsValue(_player))
            {
                PhotonNetwork.CloseConnection(_player);
            }
            
            _player = null;
            gameObject.SetActive(false);
        }

        #endregion
    }
}

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
        [SerializeField] private Button back;

        #endregion

        #region Private Fields

        private Player _player;

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
                Debug.Log("Add friend" + _player.NickName);
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

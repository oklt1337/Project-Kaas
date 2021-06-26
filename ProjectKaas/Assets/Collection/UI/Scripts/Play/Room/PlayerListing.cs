using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

namespace Collection.UI.Scripts.Play.Room
{
    public class PlayerListing : MonoBehaviourPun
    {
        #region Private Serializable Fields

        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI playerPing;
        [SerializeField] private RawImage playerPingImage;

        #endregion
        
        #region Public Field

        public Player PhotonPlayer { get; set; }
        public TextMeshProUGUI PlayerName => playerName;
        public TextMeshProUGUI PlayerPing => playerPing;
        public RawImage PlayerPingImage => playerPingImage;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClickPlayerInfo);
        }

        #endregion

        #region Public Methods

        public void ApplyPhotonPlayer(Player photonPlayer)
        {
            PhotonPlayer = photonPlayer;
            PlayerName.text = photonPlayer.NickName;
            PlayerPing.text = null;
        }

        public void OnClickPlayerInfo()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonPlayer.IsLocal)
                {
                    Debug.Log("You cant interact with yourself.");
                    return;
                }
                Debug.Log("Player info of: " + PhotonPlayer.NickName);
                OverlayCanvases.Instance.PlayerInfoCanvas.SetActive(true);
                OverlayCanvases.Instance.PlayerInfoCanvas.GetComponent<PlayerInfoCanvas>().Initialize(PhotonPlayer);
            }
        }

        #endregion
    }
}

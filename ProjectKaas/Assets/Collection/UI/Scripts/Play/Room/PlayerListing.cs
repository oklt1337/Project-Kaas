using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public Photon.Realtime.Player PhotonPlayer { get; set; }
        public TextMeshProUGUI PlayerName => playerName;
        public TextMeshProUGUI PlayerPing => playerPing;
        public RawImage PlayerPingImage => playerPingImage;

        #endregion

        #region Public Methods

        public void ApplyPhotonPlayer(Photon.Realtime.Player photonPlayer)
        {
            PlayerName.text = photonPlayer.NickName;
            PlayerPing.text = null;
        }

        #endregion
    }
}

using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Rooms
{
    public class PlayerListing : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI playerName;
        [SerializeField] TextMeshProUGUI ping;
        [SerializeField] RawImage pingIcon;
        
        public Player Player { get; private set; }

        public void SetPlayerInfo(Player player)
        {
            Player = player;

            playerName.text = player.NickName;
        }
    }
}

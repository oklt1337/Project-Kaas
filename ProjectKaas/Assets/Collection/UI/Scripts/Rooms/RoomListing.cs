using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class RoomListing : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roomName;
        [SerializeField] private TextMeshProUGUI playerCount;

        public RoomInfo RoomInfo { get; private set; }

        public void SetRoomInfo(RoomInfo info)
        {
            RoomInfo = info;
            roomName.text = info.Name;
            playerCount.text = info.PlayerCount + " / " + info.MaxPlayers;
        }
    }
}

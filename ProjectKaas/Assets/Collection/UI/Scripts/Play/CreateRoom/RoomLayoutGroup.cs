using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.UI.Scripts.Play.CreateRoom
{
    public class RoomLayoutGroup : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("Prefabs for the room display in scroll view.")]
        [SerializeField] private GameObject roomListingPrefab;

        #endregion

        #region Private Field

        #endregion

        #region Public Fields

        public GameObject RoomListingPrefab => roomListingPrefab;

        public List<RoomListing> RoomListingButtons { get; } = new List<RoomListing>();

        #endregion

        #region Private Methods

        private void RoomReceived(RoomInfo room)
        {
            var index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);

            if (index == -1)
            {
                if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
                {
                    var roomListingObj = Instantiate(RoomListingPrefab, transform, false);
                    var roomListing = roomListingObj.GetComponent<RoomListing>();

                    RoomListingButtons.Add(roomListing);

                    index = RoomListingButtons.Count - 1;
                }
            }

            if (index != -1)
            {
                var roomListing = RoomListingButtons[index];
                roomListing.SetRoomText(room.Name);
                roomListing.SetPlayerCount(room.PlayerCount, room.MaxPlayers);
                roomListing.Updated = true;
            }
        }

        private void RemoveOldRooms()
        {
            var removeRooms = new List<RoomListing>();

            foreach (var roomListing in RoomListingButtons)
            {
                if (!roomListing.Updated)
                {
                    removeRooms.Add(roomListing);
                }
                else
                {
                    roomListing.Updated = false;
                }
            }

            foreach (var roomListing in removeRooms)
            {
                var roomListingObj = roomListing.gameObject;
                RoomListingButtons.Remove(roomListing);
                Destroy(roomListingObj);
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            foreach (var room in roomList)
            {
                RoomReceived(room);
            }
            RemoveOldRooms();
        }

        #endregion
    }
}

using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class RoomListingMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private RoomListing roomListingPrefab;
        [SerializeField] private Transform content;

        private List<RoomListing> _roomListings = new List<RoomListing>();
        private RoomCanvases _roomCanvases;
        
        public void Initialize(RoomCanvases canvases)
        {
            _roomCanvases = canvases;
        }

        /// <summary>
        /// Photon internal method
        /// </summary>
        public override void OnJoinedRoom()
        {
            _roomCanvases.CurrentRoomCanvas.ActivateRoom();
        }

        /// <summary>
        /// Photon internal method
        /// </summary>
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (var info in roomList)
            {
                //removed from rooms list
                if (info.RemovedFromList)
                {
                    var index = _roomListings.FindIndex(x => x.RoomInfo.Name == info.Name);
                    if (index == -1) continue;
                    
                    //Destroy gameobject
                    Destroy(_roomListings[index].gameObject);
                    //Remove from list
                    _roomListings.RemoveAt(index);
                }
                //Added to rooms list
                else
                {
                    var listing = Instantiate(roomListingPrefab, content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _roomListings.Add(listing);
                    }
                }
            }
        }
    }
}

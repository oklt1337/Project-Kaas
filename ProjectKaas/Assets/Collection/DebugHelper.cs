using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace Collection
{
    public class DebugHelper : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log($"Is in lobby = {PhotonNetwork.InLobby}");
                if (PhotonNetwork.InLobby)
                {
                    Debug.Log(PhotonNetwork.CurrentLobby);
                }
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                Debug.Log($"Is in room = {PhotonNetwork.InRoom}");
                if (PhotonNetwork.InRoom)
                {
                    Debug.Log(PhotonNetwork.CurrentRoom);
                }
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                Debug.Log($"server: {PhotonNetwork.Server}"); 
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                Debug.Log($"count of rooms = {PhotonNetwork.CountOfRooms}");
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Debug.Log($"IsConnectedAndReady = {PhotonNetwork.IsConnectedAndReady}");
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                Debug.Log($"IsConnected = {PhotonNetwork.IsConnected}");
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                
            }
        }
    }
}

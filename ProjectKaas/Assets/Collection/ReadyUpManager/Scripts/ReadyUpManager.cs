using System.Collections.Generic;
using Collection.UI.Scripts.Play.Room;
using Unity.VisualScripting;
using UnityEngine;

namespace Collection.ReadyUpManager.Scripts
{
    public class ReadyUpManager : MonoBehaviour
    {
        #region Public Singleton

        public static ReadyUpManager Instance;

        #endregion

        #region Public Fields

        public int LobbySize { get; set; }
        
        private List<PlayerListing> ReadyPlayer { get; } = new List<PlayerListing>();

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void AddReadyPlayer(PlayerListing playerListing)
        {
            if (!ReadyPlayer.Contains(playerListing))
            {
                ReadyPlayer.Add(playerListing);
            }

            if (ReadyPlayer.Count == LobbySize)
            {
                Debug.Log("All Ready");
                //StartGameCountDown
            }
        }
        
        public void RemoveReadyPlayer(PlayerListing playerListing)
        {
            if (ReadyPlayer.Contains(playerListing))
            {
                ReadyPlayer.Remove(playerListing);
            }

            if (ReadyPlayer.Count != LobbySize)
            {
                Debug.Log("Sb is not ready anymore.");
                //StopGameCountDown
            }
        }
        
        #endregion
    }
}

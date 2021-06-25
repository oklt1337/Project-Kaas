using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play.CreateRoom
{
    public class RoomListing : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("TextMeshPro text obj for room name.")]
        [SerializeField] private TextMeshProUGUI roomNameText;
        
        [Tooltip("TextMeshPro text obj for player count.")]
        [SerializeField] private TextMeshProUGUI playerCount;

        #endregion

        #region Public Fields

        public TextMeshProUGUI RoomNameTextText => roomNameText;
        public TextMeshProUGUI PlayerCount => playerCount;
        
        public bool Updated { get; set; }
        
        public string RoomName { get; private set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            var roomListCanvasObj = OverlayCanvases.Instance.RoomListCanvas.gameObject;

            if (roomListCanvasObj == null)
                return;

            var roomListCanvas = roomListCanvasObj.GetComponent<RoomListCanvas>();

            var button = GetComponent<Button>();
            button.onClick.AddListener(() => roomListCanvas.OnClickJoinRoom(RoomNameTextText.text));
        }

        private void OnDestroy()
        {
            var button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }

        #endregion
        
        #region Public Methods

        public void SetRoomText(string rName)
        {
            RoomName = rName;
            RoomNameTextText.text = RoomName;
        }

        public void SetPlayerCount(int currentCount, int maxPlayer)
        {
            PlayerCount.text = currentCount + "/" + maxPlayer;
        }
        
        #endregion
    }
}

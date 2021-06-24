using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.Play
{
    public class RoomListing : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("TextMeshPro text obj for room name.")]
        [SerializeField] private TextMeshProUGUI roomName;
        
        [Tooltip("TextMeshPro text obj for player count.")]
        [SerializeField] private TextMeshProUGUI playerCount;

        #endregion

        #region Public Fields

        public TextMeshProUGUI RoomName => roomName;
        
        public TextMeshProUGUI PlayerCount => playerCount;
        
        public bool Updated { get; set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            var roomListCanvasObj = OverlayCanvases.Instance.RoomListCanvas.gameObject;

            if (roomListCanvasObj == null)
                return;

            var roomListCanvas = roomListCanvasObj.GetComponent<RoomListCanvas>();

            var button = GetComponent<Button>();
            button.onClick.AddListener(() => roomListCanvas.OnClickJoinRoom(RoomName.text));
        }

        private void OnDestroy()
        {
            var button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }

        #endregion
    }
}

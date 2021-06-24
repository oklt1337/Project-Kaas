using UnityEngine;

namespace Collection.UI.Scripts.Play
{
    public class RoomLayoutGroup : MonoBehaviour
    {
        #region Private Serializable Fields

        [Tooltip("Prefabs for the room display in scroll view.")]
        [SerializeField] private GameObject roomListingPrefab;

        #endregion

        #region Public Fields

        public GameObject RoomListingPrefab => roomListingPrefab;

        #endregion

        #region Public Methods

        

        #endregion
    }
}

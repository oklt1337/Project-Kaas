using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Collection.UI.Scripts.MainMenu.FriendList
{
    public class FriendInfoCanvas : MonoBehaviour
    {
        #region Private Serializable Fields
    
        [SerializeField] private Button profile;
        [SerializeField] private Button chat;
        [SerializeField] private Button remove;
        [SerializeField] private Button back;
    
        #endregion
    
        #region Private Fields
    
        private FriendInfo _friend;
    
        #endregion
    
        #region Public Methods
    
        public void Initialize(FriendInfo friend)
        {
            Debug.Log("Init Player Info");
            _friend = friend;
            profile.onClick.AddListener(OnClickProfile);
            chat.onClick.AddListener(OnClickChat);
            remove.onClick.AddListener(OnClickRemove);
        }
    
        public void OnClickBack()
        {
            _friend = null;
            gameObject.SetActive(false);
        }
    
        #endregion
    
        #region Private Methods

        private void OnClickProfile()
        {
            // open profile
            
            _friend = null;
            gameObject.SetActive(false);
        }
        
        private void OnClickChat()
        {
            //open chat
            
            _friend = null;
            gameObject.SetActive(false);
        }
    
        private void OnClickRemove()
        {
            //del friend
                
            _friend = null;
            gameObject.SetActive(false);
        }
    
        #endregion
    }
}

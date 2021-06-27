using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.Login
{
    public class AuthUIManager : MonoBehaviour
    {
        #region Public Singleton
        
        public static AuthUIManager Instance;
        
        #endregion

        #region Private Serializable Fields

        [Header("References")] 
        
        [Tooltip("Login Canvas.")]
        [SerializeField] private LoginCanvas loginCanvas;
        
        [Tooltip("Register Canvas.")]
        [SerializeField] private RegisterCanvas registerCanvas;
        
        
        [SerializeField] private GameObject checkingForAccountUI;
        [SerializeField] private GameObject verifyEmailUI;
        [SerializeField] private TMP_Text verifyEmailText;

        #endregion
        
        #region Public Fields

        public LoginCanvas LoginCanvas => loginCanvas;
        public RegisterCanvas RegisterCanvas => registerCanvas;

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

        #region Private Methods

        private void ClearUI()
        {
            LoginCanvas.gameObject.SetActive(false);
            RegisterCanvas.gameObject.SetActive(false);
            //FirebaseAuthManager.Instance.ClearOutputs();
        }

        #endregion

        #region Public Methods

        public void LoginScreen()
        {
            ClearUI();
            LoginCanvas.gameObject.SetActive(true);
        }
        
        public void RegisterScreen()
        {
            ClearUI();
            RegisterCanvas.gameObject.SetActive(true);
        }

        #endregion
    }
}

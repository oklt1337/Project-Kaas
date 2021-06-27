using UnityEngine;


namespace Collection.Authentication.Scripts
{
    public class FirebaseAuthManager : MonoBehaviour
    {
        /*
        #region Public Singleton

        public static FirebaseAuthManager Instance;

        #endregion

        #region Public Fields

        [Header("Firebase")] 
        public FirebaseAuth Auth;
        public FirebaseUser User;

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
                Destroy(Instance.gameObject);
                Instance = this;
            }

            // Check and fix dependencies
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(checkDependency =>
            {
                var dependencyStatus = checkDependency.Result;

                // check if worked.
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // if worked.
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }

        #endregion

        #region Public CoRoutines

        /// <summary>
        /// Coroutine to login.
        /// </summary>
        /// <param name="email">string</param>
        /// <param name="password">string</param>
        /// <returns></returns>
        public IEnumerator LoginCo(string email, string password)
        {
            // Convert strings to credentials.
            var credential = EmailAuthProvider.GetCredential(email, password);

            // hold as task to wait until its complete
            var loginTask = Auth.SignInWithCredentialAsync(credential);

            yield return new WaitUntil(() => loginTask.IsCompleted);

            // Check if sth went wrong
            if (loginTask.Exception != null)
            {
                // Get exception.
                var firebaseException = (FirebaseException) loginTask.Exception.GetBaseException();

                // Convert into AuthError.
                var error = (AuthError) firebaseException.ErrorCode;

                var errorText = AuthErrorHandler.GetAuthError(error);

                AuthUIManager.Instance.LoginCanvas.LoginOutputText.text = errorText;
            }
            else
            {
                if (User.IsEmailVerified)
                {
                    yield return new WaitForSeconds(1f);
                    AuthUIManager.Instance.LoginCanvas.gameObject.SetActive(false);
                }
                else
                {
                    //TODO: Send Verification Email
                    
                    // Temporary
                    AuthUIManager.Instance.LoginCanvas.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Coroutine to register.
        /// </summary>
        /// <param name="username">string</param>
        /// <param name="email">string</param>
        /// <param name="password">string</param>
        /// <param name="confirmPassword">string</param>
        /// <returns></returns>
        public IEnumerator RegistryCo(string username, string email, string password, string confirmPassword)
        {
            if (username == "" || username == String.Empty)
            {
                AuthUIManager.Instance.RegisterCanvas.RegistryOutputText.text = "Please Enter A Username";
            }
            else if (username.ToLower() == "hitler")
            {
                AuthUIManager.Instance.RegisterCanvas.RegistryOutputText.text = "Invalid Username";
            }
            else if (password != confirmPassword)
            {
                AuthUIManager.Instance.RegisterCanvas.RegistryOutputText.text = "Passwords Do Not Match";
            }
            else
            {
                var registryTask = Auth.CreateUserWithEmailAndPasswordAsync(email, password);

                yield return new WaitUntil(() => registryTask.IsCompleted);
                
                // Check if sth went wrong
                if (registryTask.Exception != null)
                {
                    // Get exception.
                    var firebaseException = (FirebaseException) registryTask.Exception.GetBaseException();

                    // Convert into AuthError.
                    var error = (AuthError) firebaseException.ErrorCode;

                    var errorText = AuthErrorHandler.GetAuthError(error);

                    AuthUIManager.Instance.RegisterCanvas.RegistryOutputText.text = errorText;
                }
                else
                {
                    // Create Profile
                    var profile = new UserProfile
                    {
                        DisplayName = username,
                        //TODO: Give Profile Default Photo
                    };

                    var createProfileTask = User.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(() => createProfileTask.IsCompleted);
                    
                    // Check if sth went wrong
                    if (createProfileTask.Exception != null)
                    {
                        // Delete profile if failed.
                        User.DeleteAsync();
                        
                        // Get exception.
                        var firebaseException = (FirebaseException) createProfileTask.Exception.GetBaseException();

                        // Convert into AuthError.
                        var error = (AuthError) firebaseException.ErrorCode;

                        var errorText = AuthErrorHandler.GetAuthError(error);

                        AuthUIManager.Instance.RegisterCanvas.RegistryOutputText.text = errorText;
                    }
                    else
                    {
                        Debug.Log($"FireBase User Created Successfully: {User.DisplayName} {User.UserId}");
                        
                        //TODO: Send Verification Email.
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize Firebase.
        /// </summary>
        private void InitializeFirebase()
        {
            Auth = FirebaseAuth.DefaultInstance;

            Auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        /// <summary>
        /// Set user.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="eventArgs">EventArgs</param>
        private void AuthStateChanged(object sender, EventArgs eventArgs)
        {
            // Check if Auth.CurrentUser != User
            // if its true auth state changed but user is the same.
            if (Auth.CurrentUser != User)
            {
                // Check if user is signed in
                var signedIn = User != Auth.CurrentUser && Auth.CurrentUser != null;

                // if is not Debug
                if (!signedIn && User != null)
                {
                    Debug.Log("Signed Out");
                }

                // if is signed in set user to current user.
                User = Auth.CurrentUser;

                // Debug singed in.
                if (signedIn)
                {
                    Debug.Log($"Singed In: {User.DisplayName}");
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears outputs shown on screen.
        /// </summary>
        public void ClearOutputs()
        {
            AuthUIManager.Instance.LoginCanvas.ClearUI();
            AuthUIManager.Instance.RegisterCanvas.ClearUI();
        }

        #endregion*/
    }
}

using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

namespace Collection.FriendList.Scripts
{
    public class FriendRequester : MonoBehaviourPun
    {
        #region Public Singleton

        public static FriendRequester Instance;

        #endregion

        #region UnityEvents

        public static readonly UnityEvent<string> OnSendFriendRequest = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnFriendRequestDenied = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnFriendRequestAccepted = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnRemoveSuccess = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnRemoveFailed = new UnityEvent<string>();
        public static readonly UnityEvent<string> OnAddFriendFailed = new UnityEvent<string>();

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

        /// <summary>
        /// Send FriendRequest.
        /// </summary>
        /// <param name="friendID">string</param>
        public static void SendFriendRequest(string friendID)
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                {
                    FunctionName = "SendFriendRequest",
                    FunctionParameter = new
                    {
                        FriendPlayFabId = friendID
                    }
                },
                result =>
                {
                    Debug.Log(result.FunctionResult);
                    OnSendFriendRequest?.Invoke(friendID);
                },
                error =>
                {
                    Debug.LogError($"Failed execute Cloud Script: {error.ErrorMessage}");
                    OnAddFriendFailed?.Invoke(error.ErrorMessage);
                });
        }

        public static void AcceptFriendRequest(string friendID)
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                {
                    FunctionName = "AcceptFriendRequest",
                    FunctionParameter = new
                    {
                        FriendPlayFabId = friendID
                    }
                },
                result =>
                {
                    Debug.Log(result.FunctionResult);
                    OnFriendRequestDenied?.Invoke(friendID);
                },
                error =>
                {
                    Debug.LogError($"Failed execute Cloud Script: {error.ErrorMessage}");
                });
        }
        
        public static void DenyFriendRequest(string friendID)
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                {
                    FunctionName = "DenyFriendRequest",
                    FunctionParameter = new
                    {
                        FriendPlayFabId = friendID
                    }
                },
                result =>
                {
                    Debug.Log(result.FunctionResult);
                    OnFriendRequestAccepted?.Invoke(friendID);
                },
                error =>
                {
                    Debug.LogError($"Failed execute Cloud Script: {error.ErrorMessage}");
                });
        }

        public static void DeleteFriend(string id)
            {
                PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
                    {
                        FriendPlayFabId = id
                    },
                    result =>
                    {
                        Debug.Log("Friend removed");
                        OnRemoveSuccess?.Invoke(id);
                    },
                    error =>
                    {
                        Debug.Log($"Remove friend Failed: {error.ErrorMessage}");
                        OnRemoveFailed?.Invoke(id);
                    });
            }

            #endregion
        }
    }

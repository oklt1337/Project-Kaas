using System;
using _Project.Scripts.PlayFab;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.Photon
{
    public class PhotonChatController : MonoBehaviour, IChatClientListener
    {
        public static PhotonChatController Instance;
        
        #region Private Serializable Fields

        #endregion

        #region Private Fields

        private ChatClient _chatClient;

        #endregion

        #region Public Fields

        #endregion

        #region Events

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            PlayFabLogin.Instance.OnLoginSuccess += ConnectToPhotonChat;
            
            _chatClient = new ChatClient(this);
        }

        private void Update()
        {
            // make sure we receive massages and being connected.
            _chatClient.Service();
        }

        private void OnDestroy()
        {
            PlayFabLogin.Instance.OnLoginSuccess -= ConnectToPhotonChat;
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Connecting Client to Photon Chat.
        /// </summary>
        private void ConnectToPhotonChat()
        {
            Debug.Log("Connecting to Photon Chat.");
            _chatClient.AuthValues = new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName);
            _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion, _chatClient.AuthValues);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sending Private Message.
        /// </summary>
        /// <param name="recipient">string</param>
        /// <param name="message">string</param>
        public void SendDirectMessage(string recipient, string message)
        {
            _chatClient.SendPrivateMessage(recipient, message);
        }
        
        #endregion

        #region Photon Chat Callbacks
        
        public void DebugReturn(DebugLevel level, string message)
        {
            switch (level)
            {
                case DebugLevel.OFF:
                    Debug.Log($"New OFF Debug: {message}");
                    break;
                case DebugLevel.ERROR:
                    Debug.LogError($"ERROR: {message}");
                    break;
                case DebugLevel.WARNING:
                    Debug.LogWarning($"Warning: {message}");
                    break;
                case DebugLevel.INFO:
                    Debug.Log($"New INFO Debug: {message}");
                    break;
                case DebugLevel.ALL:
                    Debug.Log($"New ALL Debug: {message}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public void OnDisconnected()
        {
            Debug.Log("Disconnected from Chat.");
        }

        public void OnConnected()
        {
            Debug.Log("Connected to Chat.");
        }

        public void OnChatStateChange(ChatState state)
        {
            Debug.Log($"ChatState changed to {state}.");
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            Debug.Log($"Received new Message in Channel {channelName}");
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            Debug.Log($"Received new Private Message in Channel {channelName}");
            if (!string.IsNullOrEmpty(message.ToString()))
            {
                // Channel Name format [Sender : Recipient]
                var splitNames = channelName.Split(':');
                var senderName = splitNames[0];

                if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log($"{sender}: {message}");
                }
            }
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            Debug.Log("Subscribed to new channel.");
        }

        public void OnUnsubscribed(string[] channels)
        {
            Debug.Log("Unsubscribed from a channel.");
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            Debug.Log($"Updated Status of {user}: {status}");
        }

        public void OnUserSubscribed(string channel, string user)
        {
            Debug.Log($"{user} subscribed to new channel: {channel}.");
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
            Debug.Log($"{user} unsubscribed from channel: {channel}.");
        }

        #endregion
    }
}

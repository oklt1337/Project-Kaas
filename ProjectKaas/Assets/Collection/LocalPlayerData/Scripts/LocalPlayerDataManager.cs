using System;
using UnityEngine;

namespace Collection.LocalPlayerData.Scripts
{
    public enum PlayerDataConst
    {
        DisplayName,
        UserName,
        Password,
    }
    
    public class LocalPlayerDataManager : MonoBehaviour
    {
        #region Public Static Methods

        /// <summary>
        /// Clear Local User Data
        /// </summary>
        public static void DeleteLocalUseData()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// Deletes one Obj from playerData.
        /// </summary>
        /// <param name="key">string</param>
        public static void DeleteObjOfLocalUserData(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }

        #region LoginData

        /// <summary>
        /// Set LocalUserLoginData
        /// </summary>
        /// <param name="userName">string</param>
        /// <param name="password">string</param>
        public static void SaveLoginData(string userName, string password)
        {
            PlayerPrefs.SetString(PlayerDataConst.UserName.ToString(), userName);
            PlayerPrefs.SetString(PlayerDataConst.Password.ToString(), password);
            PlayerPrefs.Save();
        }

        public static void DeleteLoginData()
        {
            if (PlayerPrefs.HasKey(PlayerDataConst.UserName.ToString()) && PlayerPrefs.HasKey(PlayerDataConst.Password.ToString()))
            {
                PlayerPrefs.DeleteKey(PlayerDataConst.UserName.ToString());
                PlayerPrefs.DeleteKey(PlayerDataConst.Password.ToString());
                PlayerPrefs.Save();
            }
        }
        
        public static string GetUserName()
        {
           return PlayerPrefs.GetString(PlayerDataConst.UserName.ToString());
        }
        
        public static string GetPassword()
        {
            return PlayerPrefs.GetString(PlayerDataConst.Password.ToString());
        }

        #endregion

        #endregion
    }
}

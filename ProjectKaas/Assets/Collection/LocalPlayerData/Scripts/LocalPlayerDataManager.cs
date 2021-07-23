using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collection.LocalPlayerData.Scripts
{
    public enum PlayerDataConst
    {
        DisplayName,
        UserName,
        Password,
        MasterAudio,
        SfxAudio,
        MusicAudio,
        BackgroundMusic,
        AmbientSound
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
            if (PlayerPrefs.HasKey(PlayerDataConst.UserName.ToString()) &&
                PlayerPrefs.HasKey(PlayerDataConst.Password.ToString()))
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

        #region AudioData

        public static Dictionary<string, float> GetAudioData()
        {
            var master = PlayerPrefs.GetFloat(PlayerDataConst.MasterAudio.ToString());
            var sfx = PlayerPrefs.GetFloat(PlayerDataConst.SfxAudio.ToString());
            var music = PlayerPrefs.GetFloat(PlayerDataConst.MusicAudio.ToString());
            var backgroundMusic = PlayerPrefs.GetFloat(PlayerDataConst.BackgroundMusic.ToString());
            var ambientSound = PlayerPrefs.GetFloat(PlayerDataConst.AmbientSound.ToString());
            
            var audioData = new Dictionary<string, float>
            {
                {PlayerDataConst.MasterAudio.ToString(), master},
                {PlayerDataConst.SfxAudio.ToString(), sfx},
                {PlayerDataConst.MusicAudio.ToString(), music},
                {PlayerDataConst.BackgroundMusic.ToString(), backgroundMusic},
                {PlayerDataConst.AmbientSound.ToString(), ambientSound}
            };

            return audioData;
        }

        public static void SaveAudioData(float master, float sfx, float music, float backgroundMusic, float ambientSound)
        {
            PlayerPrefs.SetFloat(PlayerDataConst.MasterAudio.ToString(), master);
            PlayerPrefs.SetFloat(PlayerDataConst.SfxAudio.ToString(), sfx);
            PlayerPrefs.SetFloat(PlayerDataConst.MusicAudio.ToString(), music);
            PlayerPrefs.SetFloat(PlayerDataConst.BackgroundMusic.ToString(), backgroundMusic);
            PlayerPrefs.SetFloat(PlayerDataConst.AmbientSound.ToString(), ambientSound);
            PlayerPrefs.Save();
        }

        #endregion

        #endregion
    }
}

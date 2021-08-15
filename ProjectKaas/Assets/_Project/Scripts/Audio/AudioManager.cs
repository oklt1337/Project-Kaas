using Collection.LocalPlayerData.Scripts;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Audio
{
    public enum AudioVariables
    {
        MasterVolume,
        MusicVolume,
        SfxVolume,
        AmbientSound
    }

    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager Instance;

        #endregion

        #region Private Serializabel Fields

        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private AudioSource audioSource;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            SetSavedVolumes();
        }

        #endregion

        #region Private Methods

        private void SetSavedVolumes()
        {
            var audioData = LocalPlayerDataManager.GetAudioData();

            masterMixer.SetFloat(AudioVariables.MasterVolume.ToString(),
                audioData.ContainsKey(PlayerDataConst.MasterAudio.ToString())
                    ? audioData[PlayerDataConst.MasterAudio.ToString()]
                    : -30f);

            masterMixer.SetFloat(AudioVariables.MusicVolume.ToString(),
                audioData.ContainsKey(PlayerDataConst.MusicAudio.ToString())
                    ? audioData[PlayerDataConst.MusicAudio.ToString()]
                    : -30f);
            
            masterMixer.SetFloat(AudioVariables.SfxVolume.ToString(),
                audioData.ContainsKey(PlayerDataConst.SfxAudio.ToString())
                    ? audioData[PlayerDataConst.SfxAudio.ToString()]
                    : -30f);
            
            masterMixer.SetFloat(AudioVariables.AmbientSound.ToString(),
                audioData.ContainsKey(PlayerDataConst.AmbientSound.ToString())
                    ? audioData[PlayerDataConst.AmbientSound.ToString()]
                    : -30f);
        }

        #endregion

        #region Public Methods

        public void SetMasterVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.MasterVolume.ToString(),vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetAmbientSoundVolume());
        }

        public void SetMusicVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.MusicVolume.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetAmbientSoundVolume());
        }

        public void SetSfxVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.SfxVolume.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetAmbientSoundVolume());
        }

        public void SetAmbientSoundVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.AmbientSound.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetAmbientSoundVolume());
        }

        public float GetMasterVolume()
        {
            masterMixer.GetFloat(AudioVariables.MasterVolume.ToString(), out var vol);

            return vol;
        }

        public float GetMusicVolume()
        {
            masterMixer.GetFloat(AudioVariables.MusicVolume.ToString(), out var vol);

            return vol;
        }

        public float GetSfxVolume()
        {
            masterMixer.GetFloat(AudioVariables.SfxVolume.ToString(), out var vol);

            return vol;
        }

        public float GetAmbientSoundVolume()
        {
            masterMixer.GetFloat(AudioVariables.AmbientSound.ToString(), out var vol);

            return vol;
        }

        /// <summary>
        /// Sets the Music that shall be played.
        /// </summary>
        /// <param name="newSong"> The new song. </param>
        public void SetMusic(AudioClip newSong)
        {
            audioSource.clip = newSong;
            audioSource.Play();
        }

        #endregion
    }
}

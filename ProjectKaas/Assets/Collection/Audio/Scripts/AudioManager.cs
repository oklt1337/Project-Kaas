using Collection.LocalPlayerData.Scripts;
using UnityEngine;
using UnityEngine.Audio;

namespace Collection.Audio.Scripts
{
    public enum AudioVariables
    {
        MasterVolume,
        MusicVolume,
        BackgroundMusic,
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

            var audioData = LocalPlayerDataManager.GetAudioData();

            if (audioData.ContainsKey(PlayerDataConst.MasterAudio.ToString()) &&
                audioData.ContainsKey(PlayerDataConst.MusicAudio.ToString()) &&
                audioData.ContainsKey(PlayerDataConst.SfxAudio.ToString()) &&
                audioData.ContainsKey(PlayerDataConst.AmbientSound.ToString()) && 
                audioData.ContainsKey(PlayerDataConst.BackgroundMusic.ToString()))
            {
                masterMixer.SetFloat(AudioVariables.MasterVolume.ToString(),
                    audioData[PlayerDataConst.MasterAudio.ToString()]);
                masterMixer.SetFloat(AudioVariables.MusicVolume.ToString(),
                    audioData[PlayerDataConst.MusicAudio.ToString()]);
                masterMixer.SetFloat(AudioVariables.SfxVolume.ToString(),
                    audioData[PlayerDataConst.SfxAudio.ToString()]);
                masterMixer.SetFloat(AudioVariables.BackgroundMusic.ToString(),
                    audioData[PlayerDataConst.BackgroundMusic.ToString()]);
                masterMixer.SetFloat(AudioVariables.AmbientSound.ToString(),
                    audioData[PlayerDataConst.AmbientSound.ToString()]);
            }
            else
            {
                masterMixer.SetFloat(AudioVariables.MasterVolume.ToString(), 1);
                masterMixer.SetFloat(AudioVariables.MusicVolume.ToString(), 1);
                masterMixer.SetFloat(AudioVariables.SfxVolume.ToString(), 1);
                masterMixer.SetFloat(AudioVariables.BackgroundMusic.ToString(), 1);
                masterMixer.SetFloat(AudioVariables.AmbientSound.ToString(), 1);
            }
        }

        #endregion

        #region Public Methods

        public void SetMasterVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.MasterVolume.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetBackgroundMusicVolume(), GetAmbientSoundVolume());
        }

        public void SetMusicVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.MusicVolume.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetBackgroundMusicVolume(), GetAmbientSoundVolume());
        }

        public void SetSfxVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.SfxVolume.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetBackgroundMusicVolume(), GetAmbientSoundVolume());
        }

        public void SetBackgroundMusicVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.BackgroundMusic.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetBackgroundMusicVolume(), GetAmbientSoundVolume());
        }

        public void SetAmbientSoundVolume(float vol)
        {
            masterMixer.SetFloat(AudioVariables.AmbientSound.ToString(), vol);

            LocalPlayerDataManager.SaveAudioData(GetMasterVolume(), GetMusicVolume(), GetSfxVolume(),
                GetBackgroundMusicVolume(), GetAmbientSoundVolume());
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

        public float GetBackgroundMusicVolume()
        {
            masterMixer.GetFloat(AudioVariables.BackgroundMusic.ToString(), out var vol);

            return vol;
        }

        public float GetAmbientSoundVolume()
        {
            masterMixer.GetFloat(AudioVariables.AmbientSound.ToString(), out var vol);

            return vol;
        }

        #endregion
    }
}

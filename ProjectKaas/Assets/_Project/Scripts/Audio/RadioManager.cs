using TMPro;
using UnityEngine;

namespace _Project.Scripts.Audio
{
    public class RadioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] songs;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private int index;

        public static AudioClip[] Songs;

        private void Awake()
        {
            Songs = songs;
        }

        private void OnEnable()
        {
            text.text = songs[index].name;
        }
        

        public void OnClickForward()
        {
            index++;

            if (index == songs.Length)
            {
                index = 0;
            }
                
            AudioManager.Instance.SetMusic(songs[index]);
            text.text = songs[index].name;
        }
        
        public void OnClickBackwards()
        {
            index--;

            if (index < 0)
            {
                index = songs.Length-1;
            }
                
            AudioManager.Instance.SetMusic(songs[index]);
            text.text = songs[index].name;
        }
    }
}

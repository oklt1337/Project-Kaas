using UnityEngine;

namespace Collection.Network.Scripts
{
    [CreateAssetMenu(menuName = "Manager/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private string gameVersion = "0.0.0";
        public string GameVersion => gameVersion;

        [SerializeField] private string nickName = "None";
        public string NickName => nickName;

        public string InitializeNickname()
        {
            if (!nickName.Contains("#"))
            {
                var value = Random.Range(1000, 9999);
                nickName += "#" + value;
            }

            return nickName;
        }
    }
}

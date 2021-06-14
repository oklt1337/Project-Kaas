using UnityEngine;

namespace Collection.Network.Scripts
{
    [CreateAssetMenu(menuName = "Singletons/MasterManager")]
    public class MasterManager : SingletonScriptableObj<MasterManager>
    {
        [SerializeField] private GameSettings gameSettings;
        public static GameSettings GameSettings => Instance.gameSettings;
    }
}

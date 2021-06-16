using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Collection.Network.Scripts
{
    [CreateAssetMenu(menuName = "Singletons/MasterManager")]
    public class MasterManager : SingletonScriptableObj<MasterManager>
    {
        [SerializeField] private GameSettings gameSettings;
        public static GameSettings GameSettings => Instance.gameSettings;

        private List<NetworkPrefab> _networkPrefabs = new List<NetworkPrefab>();

        public static GameObject NetworkInstantiation(GameObject obj, Vector3 pos, Quaternion rot)
        {
            foreach (var networkPrefab in Instance._networkPrefabs.Where(networkPrefab => networkPrefab.prefab == obj))
            {
                if (networkPrefab.path != string.Empty)
                {
                    var newObj = PhotonNetwork.Instantiate(networkPrefab.path, pos, rot);
                    return newObj;
                }

                Debug.LogError("Path is empty for gameobject name " + networkPrefab.prefab);
                return null;
            }

            return null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void PopulateNetworkPrefabs()
        {
#if UNITY_EDITOR            
            Instance._networkPrefabs.Clear();

            var result = Resources.LoadAll<GameObject>("");
            foreach (var obj in result)
            {
                if (obj.GetComponent<PhotonView>() != null)
                {
                    var path = AssetDatabase.GetAssetPath(obj);
                    Instance._networkPrefabs.Add(new NetworkPrefab(obj, path));
                }
            }
#endif
        }
    }
}

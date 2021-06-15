using System;
using UnityEngine;

namespace Collection.Network.Scripts
{
    [Serializable]
    public class NetworkPrefab
    {
        public GameObject prefab;
        public string path;

        public NetworkPrefab(GameObject obj, string newPath)
        {
            prefab = obj;
            path = ReturnPrefabPathCorrect(newPath);
        }

        private string ReturnPrefabPathCorrect(string oldPath)
        {
            var extLength = System.IO.Path.GetExtension(oldPath).Length;
            const int addLength = 10;
            var startIdx = oldPath.ToLower().IndexOf("resources", StringComparison.Ordinal);

            return startIdx == -1 ? string.Empty : oldPath.Substring(startIdx + addLength, oldPath.Length - (addLength + startIdx + extLength));
        }
    }
}

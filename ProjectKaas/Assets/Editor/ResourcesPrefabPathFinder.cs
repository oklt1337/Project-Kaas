#if UNITY_EDITOR
using Collection.Network.Scripts;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Editor
{
    public class ResourcesPrefabPathFinder : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            MasterManager.PopulateNetworkPrefabs();
        }
    }
}
#endif

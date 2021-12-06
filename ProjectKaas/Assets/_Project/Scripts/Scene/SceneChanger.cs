using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace _Project.Scripts.Scene
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private float loadingLoginSceneDelay = 3f;
        
        private Coroutine _loadingLoginSceneCo;
    
        private void Start()
        {
            LoadLoginScene();
        }
    
        private void LoadLoginScene()
        {
            if (_loadingLoginSceneCo != null)
                StopCoroutine(_loadingLoginSceneCo);

            _loadingLoginSceneCo = StartCoroutine(LoadLoginSceneCo());
        }

        private IEnumerator LoadLoginSceneCo()
        {
            yield return new WaitForSeconds(loadingLoginSceneDelay);
            PhotonNetwork.LoadLevel(1);
        }
    }
}

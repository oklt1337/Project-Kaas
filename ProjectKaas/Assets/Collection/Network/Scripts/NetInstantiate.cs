using Photon.Pun;
using UnityEditor;
using UnityEngine;

namespace Collection.Network.Scripts
{
    public class NetInstantiate : MonoBehaviour
    {
        [SerializeField] private string path;
        private static int _posIndex;
        
        private void Awake()
        {
            var pos1 = new Vector3(-151.5f, -0.2f, -55f);
            var pos2 = new Vector3(-148.5f, -0.2f, -57.5f);
            var pos3 = new Vector3(-151.5f, -0.2f, -60f);

            switch (_posIndex)
            {
                case 0:
                    Debug.Log("Car Instantiated at: " + pos1);
                    Debug.Log(path);
                    PhotonNetwork.Instantiate(path, pos1, Quaternion.identity);
                    _posIndex++;
                    break;
                case 1:
                    Debug.Log("Car Instantiated at: " + pos2);
                    PhotonNetwork.Instantiate(path, pos2, Quaternion.identity);
                    _posIndex++;
                    break;
                case 2:
                    Debug.Log("Car Instantiated at: " + pos3);
                    PhotonNetwork.Instantiate(path, pos3, Quaternion.identity);
                    _posIndex++;
                    break;
                case 3:
                    Debug.Log("Car Instantiated at: ");
                    break;
                case 4:
                    Debug.Log("Car Instantiated at: ");
                    break;
                case 5:
                    Debug.Log("Car Instantiated at: ");
                    break;
                case 6:
                    Debug.Log("Car Instantiated at: ");
                    break;
                case 7:
                    Debug.Log("Car Instantiated at: ");
                    break;
            }
        }
    }
}

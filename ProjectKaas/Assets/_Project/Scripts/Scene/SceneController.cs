using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Scene
{
    public class SceneController : MonoBehaviour
    {
        public static void LoadScene(byte index)
        {
            SceneManager.LoadScene(sceneBuildIndex: index);
        }
    }
}

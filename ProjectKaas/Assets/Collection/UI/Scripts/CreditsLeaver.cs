using UnityEngine;
using UnityEngine.SceneManagement;

namespace Collection.UI.Scripts
{
    public class CreditsLeaver : MonoBehaviour
    {
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}

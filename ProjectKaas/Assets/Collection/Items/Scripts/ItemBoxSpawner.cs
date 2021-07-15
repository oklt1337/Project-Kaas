using UnityEngine;

namespace Collection.Items.Scripts
{
    public class ItemBoxSpawner : MonoBehaviour
    {
        [SerializeField] private float delay;
        [SerializeField] private GameObject itemBox;

        private void Update()
        {
            // Checks status of box and 'respawns' the box after certain time.
            if (itemBox.activeSelf)
                return;

            delay -= Time.deltaTime;

            if (delay != 0)
                return;

            delay = 2;
            itemBox.SetActive(true);
        }
    }
}

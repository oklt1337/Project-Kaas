using UnityEngine;

namespace Collection.UI.Scripts.Utilities
{
    public static class Transforms
    {
        public static void DestroyChildren(this Transform trans, bool destroyImmediately = false)
        {
            foreach (Transform child in trans)
            {
                if (destroyImmediately)
                {
                    Object.DestroyImmediate(child.gameObject);
                }
                else
                {
                    Object.Destroy(child.gameObject);
                }
            }
        }
    }
}

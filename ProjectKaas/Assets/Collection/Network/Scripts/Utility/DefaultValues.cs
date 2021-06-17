using UnityEngine;

namespace Collection.Network.Scripts.Utility
{
    public static class DefaultValues
    {
        [Header("Player Values")] 
        private const string DefaultPlayerName = "Default";
        
        /// <summary>
        /// Get the default player name.
        /// </summary>
        /// <returns>string</returns>
        public static string GetDefaultPlayerName()
        {

            return DefaultPlayerName;
        }

        /// <summary>
        /// Get a Random 4 Digit Number to make Name unique.
        /// </summary>
        /// <returns>int</returns>
        public static int GetRandom4DigitNumber()
        {
            return Random.Range(1000, 9999);
        }
    }
}

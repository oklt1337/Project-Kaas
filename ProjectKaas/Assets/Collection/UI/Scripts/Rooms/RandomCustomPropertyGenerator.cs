using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Collection.UI.Scripts.Rooms
{
    public class RandomCustomPropertyGenerator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private Hashtable _customProperties = new Hashtable();

        private void SetCustomNumber()
        {
            var random = new System.Random();
            var result = random.Next(0, 99);
            text.text = result.ToString();

            _customProperties["RandomNumber"] = result;
            PhotonNetwork.SetPlayerCustomProperties(_customProperties);
        }
        
        public void OnClickButton()
        {
            SetCustomNumber();
        }
    }
}

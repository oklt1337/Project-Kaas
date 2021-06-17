using System;
using Photon.Pun;
using UnityEngine;

namespace Collection.NetworkPlayer.Scripts
{
    public class CameraHandler : MonoBehaviour
    {
        private enum CameraAngles
        {
            Overhead,
            FirstPerson,
            ThirdPersonHigh,
            ThirdPersonLow
        }
        
        #region Private Fields

        [SerializeField] private Camera overhead;
        [SerializeField] private Camera firstPerson;
        [SerializeField] private Camera thirdPersonHigh;
        [SerializeField] private Camera thirdPersonLow;

        private CameraAngles _cameraAngles;

        #endregion


        #region MonoBehaviour Callbacks

        private void Awake()
        {
            overhead.gameObject.SetActive(true);
            firstPerson.gameObject.SetActive(false);
            thirdPersonHigh.gameObject.SetActive(false);
            thirdPersonLow.gameObject.SetActive(false);

            _cameraAngles = CameraAngles.Overhead;
        }

        #endregion


        #region Public Methods

        public void ChangeCamera()
        {
            if (!PhotonNetwork.LocalPlayer.IsLocal) return;
            switch (_cameraAngles)
            {
                case CameraAngles.Overhead:
                    thirdPersonHigh.gameObject.SetActive(false);
                    thirdPersonLow.gameObject.SetActive(false);
                    overhead.gameObject.SetActive(false);
                    firstPerson.gameObject.SetActive(true);
                    break;
                case CameraAngles.FirstPerson:
                    thirdPersonLow.gameObject.SetActive(false);
                    overhead.gameObject.SetActive(false);
                    firstPerson.gameObject.SetActive(false);
                    thirdPersonHigh.gameObject.SetActive(true);
                    break;
                case CameraAngles.ThirdPersonHigh:
                    overhead.gameObject.SetActive(false);
                    firstPerson.gameObject.SetActive(false);
                    thirdPersonHigh.gameObject.SetActive(false);
                    thirdPersonLow.gameObject.SetActive(true);
                    break;
                case CameraAngles.ThirdPersonLow:
                    firstPerson.gameObject.SetActive(false);
                    thirdPersonHigh.gameObject.SetActive(false);
                    thirdPersonLow.gameObject.SetActive(false);
                    overhead.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private static Camera mainCamera;
        private bool isInited = false;

        private void Awake()
        {
            if (!isInited)
            {
                mainCamera = Camera.main;
                isInited = true;
            }
        }

        private void LateUpdate()
        {
            transform.forward = mainCamera.transform.forward;
        }
    }
}

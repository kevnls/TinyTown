using UnityEngine;
using System.Collections;

namespace kevnls
{

    public class LookAtCamera : MonoBehaviour
    {

        public Camera SourceCamera;

        void Update()
        {
            if (SourceCamera != null)
            {
                transform.rotation = SourceCamera.transform.rotation;
            }
        }
    }
}
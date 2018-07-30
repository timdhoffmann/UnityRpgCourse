using System;
using UnityEngine;

namespace Invector.CharacterController
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
    
        vThirdPersonController _controller;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster;
        Vector3 currentClickTarget;
        
        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            _controller = GetComponent<ThirdPersonCharacter>();
            currentClickTarget = transform.position;
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                print("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString());
                currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
            }
            _controller.Move(currentClickTarget - transform.position, false, false);
        }
    }
}



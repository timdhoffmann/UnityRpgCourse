using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Invector.CharacterController
{
    [RequireComponent(typeof (vThirdPersonController))]
    public class PlayerMovement : MonoBehaviour
    {
    
        vThirdPersonController _controller;
        CameraRaycaster _cameraRaycaster;
        Vector3 _currentClickTarget;
        
        private void Start()
        {
            _cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            Assert.IsNotNull(_cameraRaycaster);

            _controller = GetComponent<vThirdPersonController>();
            _currentClickTarget = transform.position;
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            HandlePointAndClickInput();
        }

        private void HandlePointAndClickInput ()
        {
            // Directional Movement.
            if (Input.GetButton("Fire2"))
            {
                print("Cursor raycast hit" + _cameraRaycaster.Hit.collider.gameObject.name.ToString());
                _currentClickTarget = _cameraRaycaster.Hit.point;  // So not set in default case
            }
            Vector3 targetDirection = _currentClickTarget - transform.position;
            _controller.input.x = targetDirection.x;
            _controller.input.y = targetDirection.z;
        }
    }
}



using Invector.CharacterController;
using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof (vThirdPersonController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _targetThreshold = 0.2f;
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
        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire2"))
        {
            print("Cursor raycast hit layer: " + _cameraRaycaster.LayerHit);

            switch (_cameraRaycaster.LayerHit)
            {
                case Layer.Walkable:
                    // Movement.
                    _currentClickTarget = _cameraRaycaster.Hit.point;
                    print("Hit Walkable");
                    break;
                default:
                    Debug.LogWarning("Raycasting to unhandled layer.");
                    break;
            }
        }
        Vector3 targetDirection = _currentClickTarget - transform.position;
        if (targetDirection.magnitude > _targetThreshold)
        {
            _controller.input.x = targetDirection.x;
            _controller.input.y = targetDirection.z; 
        }
            
    }
}
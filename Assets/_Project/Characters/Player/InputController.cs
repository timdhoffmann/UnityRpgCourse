using Invector.CharacterController;
using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof (vThirdPersonController))]
public class InputController : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private float _targetThreshold = 0.2f;
    [SerializeField] private bool _gamepadControlMode = false;

    vThirdPersonController _controller;
    CameraRaycaster _cameraRaycaster;
    Vector3 _currentClickTarget;
    #endregion
        
    private void Start()
    {
        _cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        _controller = GetComponent<vThirdPersonController>();
        _currentClickTarget = transform.position;

        if (_gamepadControlMode)
        {
            ToggleCursor();
        }
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // Toggle gamepad control.
        // TODO: Allow player to re-map or access via menu.
        if (Input.GetKeyDown(KeyCode.G))
        {
            _gamepadControlMode = !_gamepadControlMode;

            ToggleCursor();
        }

        if (_gamepadControlMode)
        {
            HandleGamepadInput();
        }
        else
        {
            HandleMouseAndKeyboardInput();
        }
    }

    private static void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void HandleGamepadInput()
    {
        // TODO: Implement gamepad input.
        throw new NotImplementedException();
    }

    private void HandleMouseAndKeyboardInput ()
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
using Invector.CharacterController;
using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof (vThirdPersonController))]
public class InputController : MonoBehaviour
{
    #region FIELDS
    [Header("Default Inputs")]
    [SerializeField] private string _horizontalInput = "Horizontal";
    [SerializeField] private string _verticallInput = "Vertical";
    [SerializeField] private string _moveInput = "Fire1";
    [SerializeField] private string _altMoveInput = "Fire2";
    [SerializeField] private KeyCode _controlModeInput = KeyCode.G;

    [Header("Input variables")]
    [SerializeField] private bool _gamepadControlMode = false;
    [SerializeField] private float _targetThreshold = 0.2f;

    vThirdPersonController _thirdPersonController;
    CameraRaycaster _cameraRaycaster;
    Vector3 _currentClickTarget;
    #endregion
        
    private void Start()
    {
        _cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        _thirdPersonController = GetComponent<vThirdPersonController>();
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
        if (Input.GetKeyDown(_controlModeInput))
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

    // TODO: Why protected virtual? Change to private?
    protected virtual void Update()
        {
            _thirdPersonController.UpdateMotor();                   // call ThirdPersonMotor methods               
            _thirdPersonController.UpdateAnimator();                // call ThirdPersonAnimator methods		               
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
        // TODO: Import necessary code from vThirdPersonInput.cs for click-to-move functionality.
        // Directional Movement.
        if (Input.GetButtonDown(_moveInput) || Input.GetButton(_altMoveInput))
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
            _thirdPersonController.input.x = targetDirection.x;
            _thirdPersonController.input.y = targetDirection.z; 
        }
            
    }
}
using Invector.CharacterController;
using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof (vThirdPersonController))]
public class InputController : MonoBehaviour
{
    #region FIELDS
    [Header("Default input assignments")]
    [SerializeField] private string _horizontalInput = "Horizontal";
    [SerializeField] private string _verticallInput = "Vertical";
    [SerializeField] private string _moveInput = "Fire1";
    [SerializeField] private string _altMoveInput = "Fire2";
    [SerializeField] private KeyCode _sprintInput = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpInput = KeyCode.Space;
    [SerializeField] private KeyCode _controlModeInput = KeyCode.G;

    [Header("Input variables")]
    [SerializeField] private bool _gamepadControlMode = false;
    [SerializeField] private float _targetThreshold = 0.2f;

    // vThirdPersonInput-specific stuff.
    public bool keepDirection;                          // keep the current direction in case you change the cameraState
    [Header("Camera Settings")]
    public string rotateCameraXInput = "Mouse X";
    public string rotateCameraYInput = "Mouse Y";

    #region COMPONENTS
    private vThirdPersonController _thirdPersonController;
    private CameraRaycaster _cameraRaycaster;
    private Vector3 _currentClickTarget;
    // TODO: Refactor away.
    protected vThirdPersonCamera _tpCamera;                // acess camera info    
    #endregion
    #endregion

    private void Start()
    {
        InitVariables();
        InitCharacter();

        if (_gamepadControlMode)
        {
            ToggleCursor();
        }
    }

    private void InitVariables ()
    {
        _cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        _thirdPersonController = GetComponent<vThirdPersonController>();
        Assert.IsNotNull(_thirdPersonController);

        _tpCamera = FindObjectOfType<vThirdPersonCamera>();
        Assert.IsNotNull(_tpCamera);

        _currentClickTarget = transform.position;
    }

    protected virtual void InitCharacter ()
    {
        _thirdPersonController.Init();

        _tpCamera.SetMainTarget(this.transform);
    }

    /// <summary>
    /// Fixed update is called in sync with physics.
    /// Called before Update().
    /// </summary>
    private void FixedUpdate()
    {
        // Toggles gamepad control mode.
        // TODO: Allow player to re-map or access via menu.
        if (Input.GetKeyDown(_controlModeInput))
        {
            _gamepadControlMode = !_gamepadControlMode;

            ToggleCursor();
        }
        //    cc.AirControl();
        //    CameraInput();
    }


    // TODO: Why protected virtual? Change to private?
    protected virtual void Update ()
    {
        _thirdPersonController.UpdateMotor();                   // call ThirdPersonMotor methods               
        _thirdPersonController.UpdateAnimator();                // call ThirdPersonAnimator methods		               
    }

    protected virtual void LateUpdate ()
    {
        Assert.IsNotNull(_thirdPersonController);		    
        HandleInput();
        //UpdateCameraStates();
    }

    // TODO: Refactor out what's not needed.
    protected virtual void HandleInput ()
    {
        if (_gamepadControlMode)
        {
            HandleGamepadInput();
        }
        else
        {
            if (!_thirdPersonController.lockMovement)
            {
                HandleMouseAndKeyboardInput();
                // Needed. Otherwise, character just runs straight.
                CameraInput();
            }
        }
    }

    private void HandleMouseAndKeyboardInput ()
    {
        HandleExitGameInput();

        MoveCharacter();
        //SprintInput();
        //JumpInput();
    }

    // TODO: Refactor what's not needed.
    #region Basic Locomotion Inputs      

    protected virtual void MoveCharacter ()
    {
        // Currently neeeded.
        _thirdPersonController.input.x = Input.GetAxis(_horizontalInput);
        _thirdPersonController.input.y = Input.GetAxis(_verticallInput);

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

    protected virtual void SprintInput ()
    {
        if (Input.GetKeyDown(_sprintInput))
            _thirdPersonController.Sprint(true);
        else if (Input.GetKeyUp(_sprintInput))
            _thirdPersonController.Sprint(false);
    }

    protected virtual void JumpInput ()
    {
        if (Input.GetKeyDown(_jumpInput))
            _thirdPersonController.Jump();
    }

    protected virtual void HandleExitGameInput ()
    {
        // just a example to quit the application 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Cursor.visible)
                Cursor.visible = true;
            else
                Application.Quit();
        }
    }

    #endregion

    // TODO: Refactor what's not needed.
    #region Camera Methods

    protected virtual void CameraInput ()
    {
        Assert.IsNotNull(_tpCamera);

        var Y = Input.GetAxis(rotateCameraYInput);
        var X = Input.GetAxis(rotateCameraXInput);

        _tpCamera.RotateCamera(X, Y);

        // tranform Character direction from camera if not KeepDirection
        if (!keepDirection)
            _thirdPersonController.UpdateTargetDirection(_tpCamera != null ? _tpCamera.transform : null);
        // rotate the character with the camera while strafing        
        RotateWithCamera(_tpCamera != null ? _tpCamera.transform : null);
    }

    protected virtual void UpdateCameraStates ()
    {
        // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on _tpCameraListData
        if (_tpCamera == null)
        {
            _tpCamera = FindObjectOfType<vThirdPersonCamera>();
            if (_tpCamera == null)
                return;
            if (_tpCamera)
            {
                _tpCamera.SetMainTarget(this.transform);
                _tpCamera.Init();
            }
        }
    }

    protected virtual void RotateWithCamera (Transform cameraTransform)
    {
        if (_thirdPersonController.isStrafing && !_thirdPersonController.lockMovement && !_thirdPersonController.lockMovement)
        {
            _thirdPersonController.RotateWithAnotherTransform(cameraTransform);
        }
    }

    #endregion

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

}
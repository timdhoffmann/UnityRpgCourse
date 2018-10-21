using Invector.CharacterController;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(vThirdPersonController))]
public class InputController : MonoBehaviour
{
    #region FIELDS
    [Header("Default input assignments")]
    [SerializeField] private string _horizontalInput = "Horizontal";
    [SerializeField] private string _verticallInput = "Vertical";
    [SerializeField] private string _clickToMoveInput = "Fire1";
    [SerializeField] private string _sprintInput = "Sprint";
    [SerializeField] private string _jumpInput = "Jump";
    [SerializeField] private string _controlModeInput = "ControlMode";

    [Header("Input variables")]
    [SerializeField] private bool _gamepadControlMode = false;
    [SerializeField] private readonly float _moveStopRadius = 0.2f;
    [SerializeField] private readonly float _meleeAttackStopRadius = 1f;

    private float _currentStopRadius = 0f;

    // vThirdPersonInput-specific stuff.
    public bool _keepDirection;                          // keep the current direction in case you change the cameraState
    [Header("Camera Settings")]
    public string _rotateCameraXInput = "Mouse X";
    public string _rotateCameraYInput = "Mouse Y";

    // Components.
    private vThirdPersonController _thirdPersonController;
    private CameraRaycaster _cameraRaycaster;
    private Vector3 _currentClickPoint;
    // TODO: [Input] Refactor away.
    protected vThirdPersonCamera _tpCamera;                // acess camera info
    #endregion

    private void Start()
    {
        InitVariables();
        InitThirdPersonCharacter();

        if (_gamepadControlMode)
        {
            ToggleCursor();
        }
    }

    private void InitVariables()
    {
        _cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        _thirdPersonController = GetComponent<vThirdPersonController>();
        Assert.IsNotNull(_thirdPersonController);

        _tpCamera = FindObjectOfType<vThirdPersonCamera>();
        Assert.IsNotNull(_tpCamera);

        _currentClickPoint = transform.position;
    }

    protected virtual void InitThirdPersonCharacter()
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
        if (Input.GetButtonDown(_controlModeInput))
        {
            _gamepadControlMode = !_gamepadControlMode;

            // TODO: [Input] Fix cursor visibility and Game mode going out of sync.
            ToggleCursor();

            // Clear click target.
            _currentClickPoint = transform.position;
        }
        //    cc.AirControl();
        //    CameraInput();
    }

    private void Update()
    {
        _thirdPersonController.UpdateMotor();                   // call ThirdPersonMotor methods
        _thirdPersonController.UpdateAnimator();                // call ThirdPersonAnimator methods
    }

    protected virtual void LateUpdate()
    {
        Assert.IsNotNull(_thirdPersonController);
        HandleInput();
        //UpdateCameraStates();
    }

    protected virtual void HandleInput()
    {
        if (!_thirdPersonController.lockMovement)
        {
            if (_gamepadControlMode)
            {
                HandleGamepadInput();
            }
            else
            // Mouse & keyboard mode.
            {
                HandleMouseAndKeyboardInput();
            }
            CameraInput();
        }
    }

    private void HandleMouseAndKeyboardInput()
    {
        HandleExitGameInput();

        MoveCharacter();
        //SprintInput();
        //JumpInput();
    }

    #region GAMEPAD INPUT
    private void HandleGamepadInput()
    {
        // TODO: [Input] Fix axis mapping to camera.
        _thirdPersonController.input.x = Input.GetAxis(_horizontalInput);
        _thirdPersonController.input.y = Input.GetAxis(_verticallInput);
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
    #endregion

    // TODO: [Input] Refactor what's not needed.

    #region BASIC LOCOMOTION INPUTS
    protected virtual void MoveCharacter()
    {
        // Currently neeeded.
        _thirdPersonController.input.x = 0f;
        _thirdPersonController.input.y = 0f;

        if (Input.GetButtonDown(_clickToMoveInput))
        {
            //switch (_cameraRaycaster.CurrentLayerHit)
            //{
            //    case Layer.Walkable:
            //        // Movement.
            //        Debug.Log("Clicked Walkable");
            //        _currentClickPoint = _cameraRaycaster.Hit.point;
            //        _currentStopRadius = _moveStopRadius;
            //        break;

            //    case Layer.Enemy:
            //        // Attacking.
            //        Debug.Log("Clicked enemy.");
            //        _currentClickPoint = _cameraRaycaster.Hit.point;
            //        _currentStopRadius = _meleeAttackStopRadius;
            //        break;

            //    default:
            //        Debug.LogWarning("Raycasting to unhandled layer: " + _cameraRaycaster.CurrentLayerHit);
            //        break;
            //}
        }

        Vector3 currentMoveDestination = _currentClickPoint - transform.position;

        if (currentMoveDestination.magnitude >= _currentStopRadius)
        {
            // Walk to destination.
            _thirdPersonController.input.x = currentMoveDestination.x;
            _thirdPersonController.input.y = currentMoveDestination.z;
        }
    }

    protected virtual void SprintInput()
    {
        if (Input.GetButtonDown(_sprintInput))
            _thirdPersonController.Sprint(true);
        else if (Input.GetKeyUp(_sprintInput))
            _thirdPersonController.Sprint(false);
    }

    protected virtual void JumpInput()
    {
        if (Input.GetButtonDown(_jumpInput))
            _thirdPersonController.Jump();
    }

    protected virtual void HandleExitGameInput()
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

    #region Camera Methods
    protected virtual void CameraInput()
    {
        Assert.IsNotNull(_tpCamera);

        var Y = Input.GetAxis(_rotateCameraYInput);
        var X = Input.GetAxis(_rotateCameraXInput);

        _tpCamera.RotateCamera(X, Y);

        // transform Character direction from camera if not KeepDirection
        if (!_keepDirection)
            _thirdPersonController.UpdateTargetDirection(_tpCamera != null ? _tpCamera.transform : null);
        // rotate the character with the camera while strafing
        RotateWithCamera(_tpCamera != null ? _tpCamera.transform : null);
    }

    protected virtual void UpdateCameraStates()
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

    protected virtual void RotateWithCamera(Transform cameraTransform)
    {
        if (_thirdPersonController.isStrafing && !_thirdPersonController.lockMovement)
        {
            _thirdPersonController.RotateWithAnotherTransform(cameraTransform);
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        // Movement Gizmos.
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, _currentClickPoint);

        if (_currentStopRadius == _meleeAttackStopRadius)
        {
            Gizmos.color = Color.red;
        }

        if ((_currentClickPoint - transform.position).magnitude > 0f)
        {
            Gizmos.DrawWireSphere(transform.position, _currentStopRadius);
        }
    }
}
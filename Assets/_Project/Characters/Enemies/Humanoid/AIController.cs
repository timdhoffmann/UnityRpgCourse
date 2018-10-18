using System.Collections;
using System.Collections.Generic;
using Invector.CharacterController;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    #region FIELDS

    [Header("Action variables")]
    [SerializeField]
    private Transform _targetTransform = null;
    [SerializeField]
    private readonly float _moveStopRadius = 0.2f;
    [SerializeField]
    private readonly float _meleeAttackStopRadius = 1f;

    private float _currentStopRadius = 0f;
    private bool _playerDetected = false;

    // vThirdPersonInput-specific stuff.
    public bool _keepDirection;                          // keep the current direction in case you change the cameraState

    // Components.
    private vThirdPersonController _thirdPersonController;
    //private CameraRaycaster _cameraRaycaster;
    private Vector3 _currentClickPoint;
    // TODO: [Input] Refactor away.
    //protected vThirdPersonCamera _tpCamera;                // acess camera info
    #endregion

    private void Start()
    {
        InitVariables();
        InitThirdPersonCharacter();

        _playerDetected = true;
    }

    private void InitVariables()
    {
        _thirdPersonController = GetComponent<vThirdPersonController>();
        Assert.IsNotNull(_thirdPersonController);

        _currentClickPoint = transform.position;
    }

    protected virtual void InitThirdPersonCharacter()
    {
        _thirdPersonController.Init();

        //_tpCamera.SetMainTarget(this.transform);
    }

    private void Update()
    {
        // call ThirdPersonMotor methods
        _thirdPersonController.UpdateMotor();

        // call ThirdPersonAnimator methods
        _thirdPersonController.UpdateAnimator();
    }

    protected virtual void LateUpdate()
    {
        Assert.IsNotNull(_thirdPersonController);
        HandleInput();
        //UpdateCameraStates();
    }

    protected virtual void HandleInput()
    {
        if (!_thirdPersonController.lockMovement && _playerDetected)
        {
            MoveCharacter();
            //SprintInput();
            //JumpInput();
            //CameraInput();
        }
    }

    #region BASIC LOCOMOTION INPUTS
    protected virtual void MoveCharacter()
    {
        // Currently neeeded.
        _thirdPersonController.input.x = 0f;
        _thirdPersonController.input.y = 0f;

        Vector3 currentMoveDestination = _targetTransform.position - transform.position;

        if (currentMoveDestination.magnitude >= _currentStopRadius)
        {
            // Walk to destination.
            _thirdPersonController.input.x = currentMoveDestination.x;
            _thirdPersonController.input.y = currentMoveDestination.z;
        }
    }

    //protected virtual void SprintInput()
    //{
    //    if (Input.GetButtonDown(_sprintInput))
    //        _thirdPersonController.Sprint(true);
    //    else if (Input.GetKeyUp(_sprintInput))
    //        _thirdPersonController.Sprint(false);
    //}

    //protected virtual void JumpInput()
    //{
    //    if (Input.GetButtonDown(_jumpInput))
    //        _thirdPersonController.Jump();
    //}

    #endregion

    #region Camera Methods
    //protected virtual void CameraInput()
    //{
    //    Assert.IsNotNull(_tpCamera);

    //    var Y = Input.GetAxis(_rotateCameraYInput);
    //    var X = Input.GetAxis(_rotateCameraXInput);

    //    _tpCamera.RotateCamera(X, Y);

    //    // transform Character direction from camera if not KeepDirection
    //    if (!_keepDirection)
    //        _thirdPersonController.UpdateTargetDirection(_tpCamera != null ? _tpCamera.transform : null);
    //    // rotate the character with the camera while strafing
    //    RotateWithCamera(_tpCamera != null ? _tpCamera.transform : null);
    //}

    //protected virtual void UpdateCameraStates()
    //{
    //    // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on _tpCameraListData
    //    if (_tpCamera == null)
    //    {
    //        _tpCamera = FindObjectOfType<vThirdPersonCamera>();
    //        if (_tpCamera == null)
    //            return;
    //        if (_tpCamera)
    //        {
    //            _tpCamera.SetMainTarget(this.transform);
    //            _tpCamera.Init();
    //        }
    //    }
    //}

    //protected virtual void RotateWithCamera(Transform cameraTransform)
    //{
    //    if (_thirdPersonController.isStrafing && !_thirdPersonController.lockMovement)
    //    {
    //        _thirdPersonController.RotateWithAnotherTransform(cameraTransform);
    //    }
    //}
    #endregion

    //private void OnDrawGizmos()
    //{
    //    // Movement Gizmos.
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawLine(transform.position, _currentClickPoint);

    //    if (_currentStopRadius == _meleeAttackStopRadius)
    //    {
    //        Gizmos.color = Color.red;
    //    }

    //    if ((_currentClickPoint - transform.position).magnitude > 0f)
    //    {
    //        Gizmos.DrawWireSphere(transform.position, _currentStopRadius);
    //    }
    //}
}
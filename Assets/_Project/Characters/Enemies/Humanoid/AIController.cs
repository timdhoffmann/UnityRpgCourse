using System.Collections;
using System.Collections.Generic;
using Invector.CharacterController;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(vThirdPersonController))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    #region Properties
    // the nav mesh Agent required for the path finding
    public NavMeshAgent Agent { get; private set; } = null;

    // the CharacterController we are controlling
    public vThirdPersonController CharacterController { get; private set; } = null;

    // _targetPosition to aim for
    public Vector3 TargetPosition
    {
        get => _targetPosition;
        set => _targetPosition = value;
    }
    #endregion

    #region Fields
    [SerializeField] private float playerDetectionRadius = 5.0f;

    private Vector3 _targetPosition = Vector3.zero;
    private Vector3 _returnPosition = Vector3.zero;
    private GameObject _player = null;
    #endregion

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        Agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        CharacterController = GetComponent<vThirdPersonController>();

        // Get external references.
        _player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(_player, "No GameObject with Tag 'Player' found");

        // Set default values.
        _returnPosition = transform.position;

        Agent.updateRotation = false;
        Agent.updatePosition = true;

        //CharacterController.Init();
    }

    private void Update()
    {
        TargetPosition = DetectPlayer() ? _player.transform.position : _returnPosition;

        Agent.SetDestination(TargetPosition);

        CharacterController.UpdateMotor();
        CharacterController.UpdateAnimator();
    }

    protected virtual void LateUpdate()
    {
        ControlMovement();
    }

    protected virtual void ControlMovement()
    {
        // TODO: Refactor following two lines into one, if possible.
        CharacterController.targetDirection = Agent.destination - transform.position;

        if (Agent.remainingDistance >= Agent.stoppingDistance)
        {
            // Walk to destination.
            CharacterController.input.x = CharacterController.targetDirection.x;
            CharacterController.input.y = CharacterController.targetDirection.z;
        }
        else
        {
            // Stop.
            // Currently needed, because the character controller constantly updates movement.
            CharacterController.input.x = 0f;
            CharacterController.input.y = 0f;
        }
    }

    private bool DetectPlayer()
    {
        return Vector3.Distance(transform.position, _player.transform.position) <= playerDetectionRadius;
    }
}
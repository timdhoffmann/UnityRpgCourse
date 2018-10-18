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
    // the navmesh Agent required for the path finding
    public NavMeshAgent Agent { get; private set; } = null;

    // the CharacterController we are controlling
    public vThirdPersonController CharacterController { get; private set; } = null;

    // target to aim for
    public Transform Target
    {
        get => target;
        set => target = value;
    }
    #endregion

    [SerializeField]
    private Transform target = null;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        Agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        CharacterController = GetComponent<vThirdPersonController>();

        Agent.updateRotation = false;
        Agent.updatePosition = true;

        //CharacterController.Init();
    }

    private void Update()
    {
        if (Target != null)
        {
            Agent.SetDestination(Target.transform.position);
        }

        CharacterController.UpdateMotor();
        CharacterController.UpdateAnimator();
    }

    protected virtual void LateUpdate()
    {
        MoveCharacter();
    }

    protected virtual void MoveCharacter()
    {
        CharacterController.UpdateTargetDirection(Target);

        Vector3 currentMoveDestination = Agent.steeringTarget - transform.position;

        if (Agent.remainingDistance >= Agent.stoppingDistance)
        {
            // Walk to destination.
            CharacterController.input.x = currentMoveDestination.x;
            CharacterController.input.y = currentMoveDestination.z;
        }
        else
        {
            // Stop.
            // Currently needed, because the character controller constantly updates movement.
            CharacterController.input.x = 0f;
            CharacterController.input.y = 0f;
        }
    }
}
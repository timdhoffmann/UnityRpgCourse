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
    #endregion

    // target to aim for
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
        CharacterController.UpdateMotor();
        CharacterController.UpdateAnimator();
    }

    protected virtual void LateUpdate()
    {
        if (target != null)
        {
            //Agent.SetDestination(target.position);
        }

        MoveCharacter();

        //if (Agent.remainingDistance > Agent.stoppingDistance)
        //{
        //    CharacterController.UpdateTargetDirection(target);
        //    CharacterController.targetDirection = target.transform.position;
        //    CharacterController.input.x = target.transform.position.x;
        //    CharacterController.input.y = target.transform.position.z;
        //}
        //else
        //{
        //    CharacterController.input.x = 0.0f;
        //    CharacterController.input.y = 0.0f;
        //}
    }

    protected virtual void MoveCharacter()
    {
        CharacterController.UpdateTargetDirection(target);

        // Currently neeeded.
        CharacterController.input.x = 0f;
        CharacterController.input.y = 0f;

        Vector3 currentMoveDestination = target.transform.position - transform.position;

        if (currentMoveDestination.magnitude >= Agent.stoppingDistance)
        {
            // Walk to destination.
            CharacterController.input.x = currentMoveDestination.x;
            CharacterController.input.y = currentMoveDestination.z;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
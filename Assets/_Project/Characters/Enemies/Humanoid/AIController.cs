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
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public vThirdPersonController character { get; private set; } // the character we are controlling
    public Transform target;                                    // target to aim for

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<vThirdPersonController>();

        agent.updateRotation = false;
        agent.updatePosition = true;

        character.Init();
    }

    private void Update()
    {
        if (target != null)
        {
        }

        character.UpdateMotor();
        character.UpdateAnimator();
    }

    protected virtual void LateUpdate()
    {
        if (target != null)
        {
            //agent.SetDestination(target.position);
        }

        MoveCharacter();

        //if (agent.remainingDistance > agent.stoppingDistance)
        //{
        //    character.UpdateTargetDirection(target);
        //    character.targetDirection = target.transform.position;
        //    character.input.x = target.transform.position.x;
        //    character.input.y = target.transform.position.z;
        //}
        //else
        //{
        //    character.input.x = 0.0f;
        //    character.input.y = 0.0f;
        //}
    }

    protected virtual void MoveCharacter()
    {
        character.UpdateTargetDirection(target);

        // Currently neeeded.
        character.input.x = 0f;
        character.input.y = 0f;

        Vector3 currentMoveDestination = target.transform.position - transform.position;

        if (currentMoveDestination.magnitude >= agent.stoppingDistance)
        {
            // Walk to destination.
            character.input.x = currentMoveDestination.x;
            character.input.y = currentMoveDestination.z;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}